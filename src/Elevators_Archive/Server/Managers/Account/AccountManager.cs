using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using Server.Models;
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
    
    public async Task<ManagerResult> Authenticate(string code)
    {
        try
        {
            #region Get tokens
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

            if (!response.IsSuccessStatusCode)
                return new (response.StatusCode, "There was an error retrieving discord token");
            
            var content = await response.Content.ReadAsStreamAsync();
            var doc = await JsonDocument.ParseAsync(content);
        
            var accessToken = doc.RootElement.GetProperty("access_token").ToString();
            var refreshToken = doc.RootElement.GetProperty("refresh_token").ToString();
            #endregion

            #region Get user from token
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            response = await Client.GetAsync("https://discord.com/api/users/@me");

            var jsonContent = await response.Content.ReadAsStringAsync();
        
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            };
        
            var user = JsonSerializer.Deserialize<DiscordUser>(jsonContent, options);
            #endregion

            #region Create JWT token
            List<Claim> claims = new List<Claim>
            {
                new("Name", user!.Username ?? ""),
                new("Id", user.Id ?? ""),
                new("ExpiryTimeStamp", ((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString()),
                new("Avatar", user.Avatar ?? "") //TODO: GIF, PNG, JPG
            };
        
            var key = new SymmetricSecurityKey(UTF8.GetBytes(Config.GetSection("Auth:Token").Value!));
        
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        
            var jwtToken = new JwtSecurityToken
            (
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );
            #endregion

            #region Save user to database
            try
            {
                await Connection.OpenAsync();

                // Check if user isnt already in
                await using var firstCommand = new MySqlCommand($"SELECT Count(*) FROM users WHERE user_id = '{user.Id}'", Connection);
                var result = await firstCommand.ExecuteScalarAsync();

                if (result == null || (long)result <= 0)
                {
                    // Add new values
                    await using var command = new MySqlCommand($"INSERT INTO users VALUES ('{user.Id}', '{accessToken}', '{refreshToken}');", Connection);
                    await command.ExecuteNonQueryAsync();
                }
            }
            finally
            {
                await Connection.CloseAsync();
            }
            #endregion
            
            return new(HttpStatusCode.OK, jwtToken);
        }
        catch (Exception e)
        {
            return new(HttpStatusCode.InternalServerError, e);
        }
    }
    public async Task<ManagerResult> Logout(string userId)
    {
        try
        {
            await Connection.OpenAsync();
            
            // Remove user
            await using var command = new MySqlCommand($"DELETE FROM users WHERE user_id='{userId}'", Connection);
            await command.ExecuteNonQueryAsync();

            return new(HttpStatusCode.OK);
        }
        catch (Exception e)
        {
            return new(HttpStatusCode.InternalServerError, e);
        }
        finally
        {
            await Connection.CloseAsync();
        }
    }
}