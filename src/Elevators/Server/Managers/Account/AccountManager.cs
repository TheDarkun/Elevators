using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using Shared.Models;
using static System.Text.Encoding;

namespace Server.Managers.Account;

public class AccountManager : IAccountManager
{
    private HttpClient client { get; }
    private IConfiguration config { get; }

    public AccountManager(IConfiguration config, HttpClient client)
    {
        this.config = config;
        this.client = client;
    }
    
    public async Task<string> Authenticate(string code)
    {
        try
        {
            var discordToken = await GetDiscordToken(code);
            var JwtToken = await CreateJwtToken(discordToken);
            
            return JwtToken;
        }
        catch (Exception)
        {
            throw new();
        }
    }
    
    private async Task<string> GetDiscordToken(string code)
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
        
        var token = doc.RootElement.GetProperty("access_token");

        return token.ToString();
    }
    
    private async Task<string> CreateJwtToken(string token)
    {
        var discordUser = await GetUserFromToken(token);

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
        
        var jwt = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        
        return jwt;
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