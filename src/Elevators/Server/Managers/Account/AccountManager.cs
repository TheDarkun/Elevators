using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using Shared.Models;
using static System.Text.Encoding;

namespace Server.Managers.Account;

public class AccountManager : IAccountManager
{
    private HttpClient Client { get; }
    private IConfiguration Config { get; }
    private MySqlConnection Connection { get; }
    
    public AccountManager(IConfiguration config, HttpClient client, MySqlConnection connection)
    {
        Config = config;
        Client = client;
        Connection = connection;
    }
    
    public async Task<string> Authenticate(string code)
    {
        try
        {
            // Get tuple of access token and a refresh token
            var tokens = await GetDiscordTokens(code);
            var jwtToken = await CreateJwtToken(tokens.Item1, tokens.Item2);
            
            return jwtToken;
        }
        catch (Exception)
        {
            throw new();
        }
    }

    public async Task<bool> BotIsJoined(long guildId)
    {
        Client.DefaultRequestHeaders.Clear();
        Client.DefaultRequestHeaders.Add("Authorization", $"Bot {Config.GetSection("Discord:BotToken").Value!}");
        
        var response = await Client.GetAsync($"https://discord.com/api/guilds/{guildId}");

        // 200 - Bot is on the server
        // 404 - Bot is NOT on the server
        if (response.IsSuccessStatusCode)
            return true;

        return false;
    }

    public async Task<IEnumerable<DiscordServer>> GetJoinedServers(string id)
    {
        try
        {
            // Get discord token from jwtToken
            var token = await GetUserTokenFromDatabase(id);

            if (token is null)
                throw new();

            // Make a GET request in /users/@me/guilds
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = await Client.GetAsync("https://discord.com/api/users/@me/guilds");

            var jsonContent = await response.Content.ReadAsStringAsync();

            // Deserialize result to an Immutable Array of DiscordServers
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            
            // Get only servers, where the user is either an owner or has admin permissions
            IEnumerable<DiscordServer> result =
                JsonSerializer.Deserialize<IEnumerable<DiscordServer>>(jsonContent, options)!.Where(server =>
                    server.Permissions == 2147483647);

            // Return the result
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task Logout(string userId)
    {
        try
        {
            await Connection.OpenAsync();
            
            // Remove user
            await using var command = new MySqlCommand($"DELETE FROM users WHERE user_id='{userId}'", Connection);
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            await Connection.CloseAsync();
        }
    }

    private async Task<string?> GetUserTokenFromDatabase(string id)
    {
        try
        {
            await Connection.OpenAsync();

            await using var command = new MySqlCommand($"SELECT access_token from users WHERE user_id = '{id}'", Connection);
            var result = await command.ExecuteScalarAsync();

            if (result is null)
                return null;

            var token = result.ToString();
            return token;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            await Connection.CloseAsync();
        }
    }

    private async Task<(string, string)> GetDiscordTokens(string code)
    {
        var formData = new Dictionary<string, string>
        {
            { "client_id", Config.GetSection("Discord:AppId").Value!},
            { "client_secret", Config.GetSection("Discord:AppSecret").Value!},
            { "grant_type", "authorization_code" },
            { "code", code },
            { "redirect_uri", $"{Config.GetSection("Website:Url").Value!}api/Account/Authenticate"}
        };
        var formContent = new FormUrlEncodedContent(formData);
        Client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
        var response = await Client.PostAsync("https://discord.com/api/v10/oauth2/token", formContent);
        var content = await response.Content.ReadAsStreamAsync();
        var doc = await JsonDocument.ParseAsync(content);
        
        var accessToken = doc.RootElement.GetProperty("access_token").ToString();
        var refreshToken = doc.RootElement.GetProperty("refresh_token").ToString();
        return (accessToken, refreshToken);
    }
    private async Task<string> CreateJwtToken(string accessToken, string refreshToken)
    {
        var discordUser = await GetUserFromToken(accessToken);

        if (discordUser is null)
            throw new();
        
        List<Claim> claims = new List<Claim>
        {
            new("Name", discordUser.Username ?? ""),
            new("Id", discordUser.Id ?? ""),
            new("ExpiryTimeStamp", ((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString()),
            new("Avatar", discordUser.Avatar ?? "") //TODO: GIF, PNG, JPG
        };
        
        var key = new SymmetricSecurityKey(UTF8.GetBytes(Config.GetSection("Auth:Token").Value!));
        
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        
        var jwtToken = new JwtSecurityToken
        (
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: creds
        );

        SaveUserToDatabase(discordUser.Id!, accessToken, refreshToken);
        
        var jwt = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        
        return jwt;
    }
    
    private async void SaveUserToDatabase(string userId, string accessToken, string refreshToken)
    {
        try
        {
            await Connection.OpenAsync();

            // Check if user isnt already in
            await using var firstCommand = new MySqlCommand($"SELECT Count(*) FROM users WHERE user_id = '{userId}'", Connection);
            var result = await firstCommand.ExecuteScalarAsync();

            if (result is not null && (long)result > 0)
                return;
            
            
            // Add new values
            await using var command = new MySqlCommand($"INSERT INTO users VALUES ('{userId}', '{accessToken}', '{refreshToken}');", Connection);
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            await Connection.CloseAsync();
        }
    }

    private async Task<DiscordUser?> GetUserFromToken(string token)
    {
        Client.DefaultRequestHeaders.Clear();
        Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var response = await Client.GetAsync("https://discord.com/api/users/@me");

        var jsonContent = await response.Content.ReadAsStringAsync();
        
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
        };
        
        var result = JsonSerializer.Deserialize<DiscordUser>(jsonContent, options);
        
        return result;
    }
}