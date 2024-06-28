using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using Microsoft.IdentityModel.Tokens;

namespace Elevators.Authentication;

public class AuthenticationMiddleware(
    RequestDelegate next,
    IConfiguration config,
    HttpClient client,
    ILogger<AuthenticationMiddleware> logger)
{
    private bool _uriSet;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            if (!context.Request.Cookies.ContainsKey("account"))
            {
                // Because of how middleware works, the URI has to be set only once. Otherwise, when anyone else tries to authorize after us,
                // they get rejected and we get System.InvalidOperationException: This instance has already started one or more requests.
                // Properties can only be modified before sending the first request.
                if (!_uriSet)
                {
                    var address = $"https://discord.com/api/v{config.GetValue<int>("apiVersion")}";
                    logger.LogInformation("Setting up the base address in AuthenticationMiddleware to {Address}",
                        address);
                    client.BaseAddress = new Uri(address);
                    _uriSet = true;
                }

                string? code = context.Request.Query["code"];
                if (code != null)
                {
                    var account = await Authenticate(code);
                    if (account == null)
                        return;

                    var user = await GetUser(account);
                    if (user == null)
                        return;

                    var jwtToken = CreateJwtToken(user);
                    SetJwtCookie(context, jwtToken);
                }
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Something went wrong.");
        }
        finally
        {
            await next(context);
        }
    }

    private async Task<string?> Authenticate(string code)
    {
        var formData = new Dictionary<string, string>
        {
            { "client_id", config["clientId"]! },
            { "client_secret", config["clientSecret"]! },
            { "grant_type", "authorization_code" },
            { "code", code },
            { "redirect_uri", config["redirectUri"]! }
        };

        logger.LogInformation(
            "Getting access token using \nclient_id as {ClientId} \nclient_secret as {ClientSecret} \ncode as {Code} \nredirect_uri as {RedirectUri}",
            config["clientId"], config["clientSecret"], code, config["redirectUri"]);

        var formContent = new FormUrlEncodedContent(formData);
        var response = await client.PostAsync("oauth2/token", formContent);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Failed to authenticate. Status code: {StatusCode}", response.StatusCode);
            return null;
        }
        
        var content = await response.Content.ReadAsStringAsync();
        var account = JObject.Parse(content);
        var accessToken = (string)account["access_token"]!;
        logger.LogInformation("Successfully retrieved access_token. Access token: {AccessToken}", accessToken);
        return accessToken;
    }

    private async Task<string?> GetUser(string accessToken)
    {
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        logger.LogInformation("Getting user id using access token");
        var response = await client.GetAsync("users/@me");

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Failed to retrieve user id. Status code: {StatusCode}", response.StatusCode);
            return null;
        }
        var user = JObject.Parse(await response.Content.ReadAsStringAsync());
        var userId = (string)user["id"]!;
        logger.LogInformation("Successfully retrieved user id. User id: {UserId}", userId);
        return userId;
    }

    private string CreateJwtToken(string userId)
    {
        var claims = new List<Claim>
        {
            new Claim("Id", userId),
            new Claim("exp", ((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString())
        };

        logger.LogInformation("Creating jwt token");
        
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config["randomJwtToken"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var jwtToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: creds
        );

        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(jwtToken);
    }

    private void SetJwtCookie(HttpContext context, string jwtToken)
    {
        logger.LogInformation("Setting jwt token to cookie");
        var options = new CookieOptions
        {
            Expires = DateTimeOffset.Now.AddDays(7),
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax
        };

        context.Response.Cookies.Append("account", jwtToken, options);
    }
}