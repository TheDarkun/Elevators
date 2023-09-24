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
    private HttpClient client { get; }
    private IConfiguration config { get; }
    private MySqlConnection connection { get; }
    
    public AccountManager(IConfiguration config, HttpClient client, MySqlConnection connection)
    {
        this.config = config;
        this.client = client;
        this.connection = connection;
    }
    
    public async Task<string> Authenticate(string code)
    {
        try
        {
            var tokens = await GetDiscordTokens(code);
            var JwtToken = await CreateJwtToken(tokens.Item1, tokens.Item2);
            
            return JwtToken;
        }
        catch (Exception)
        {
            throw new();
        }
    }

    public async Task<bool> BotIsJoined(long guildId)
    {
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("Authorization", $"Bot {config.GetSection("Discord:BotToken").Value!}");

        
        
        Console.WriteLine("waiting");
        
        var response = await client.GetAsync($"https://discord.com/api/guilds/{guildId}");

        if (response.IsSuccessStatusCode)
            return true;

        return false;
    }

    public async Task<IEnumerable<DiscordServer>> GetJoinedServers(string id)
    {
        // Get discord token from jwtToken
        var token = await GetUsersTokenFromDatabase(id);

        if (token is null)
            throw new ();
        
        // Make a GET request in /users/@me/guilds
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var response = await client.GetAsync("https://discord.com/api/users/@me/guilds");

        var jsonContent = await response.Content.ReadAsStringAsync();
        
        // Deserialize result to an Immutable Array of DiscordServers
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        
        IEnumerable<DiscordServer> result = JsonSerializer.Deserialize<IEnumerable<DiscordServer>>(jsonContent, options)!.Where(server => server.Permissions == 2147483647);
        
        // Return the result
        return result;
    }

    private async Task<string?> GetUsersTokenFromDatabase(string id)
    {
        await connection.OpenAsync();

        await using var command = new MySqlCommand($"SELECT access_token from Users WHERE id = '{id}'", connection);
        var result = await command.ExecuteScalarAsync();

        if (result is null)
            return null;

        var token = result.ToString();
        await connection.CloseAsync();
        return token;

        
    }

    private async Task<(string, string)> GetDiscordTokens(string code)
    {
        var formData = new Dictionary<string, string>
        {
            { "client_id", config.GetSection("Discord:AppId").Value!},
            { "client_secret", config.GetSection("Discord:AppSecret").Value!},
            { "grant_type", "authorization_code" },
            { "code", code },
            { "redirect_uri", $"{config.GetSection("Website:Url").Value!}api/Account/Authenticate"}
        };
        var formContent = new FormUrlEncodedContent(formData);
        client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
        var response = await client.PostAsync("https://discord.com/api/v10/oauth2/token", formContent);
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
        
        var key = new SymmetricSecurityKey(UTF8.GetBytes(config.GetSection("Auth:Token").Value!));
        
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

    private async void SaveUserToDatabase(string discordUserId, string accessToken, string refreshToken)
    {
        await connection.OpenAsync();
        
        await using var command = new MySqlCommand($"INSERT INTO Users VALUES ('{discordUserId}', '{accessToken}', '{refreshToken}');", connection);
        await command.ExecuteNonQueryAsync();

        await connection.CloseAsync();
    }

    private async Task<DiscordUser?> GetUserFromToken(string token)
    {

        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var response = await client.GetAsync("https://discord.com/api/users/@me");

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