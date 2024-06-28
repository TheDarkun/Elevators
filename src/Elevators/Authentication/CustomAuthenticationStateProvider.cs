using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Elevators.Authentication;

public class CustomAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var context = httpContextAccessor.HttpContext;
        
        if (context is null || !context.Request.Cookies.TryGetValue("account", out var token))
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        
        // Validate the token and extract the claims
        var claims = ValidateTokenAndExtractClaims(token);
        
        // If the token does not exist or is invalid, return a default authentication state
        if (claims is null) 
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            
        var identity = new ClaimsIdentity(claims, "jwt");
        var principal = new ClaimsPrincipal(identity);
            
        return new AuthenticationState(principal);
    }
    
    private Claim[]? ValidateTokenAndExtractClaims(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = GetValidationParameters();

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal.Claims.ToArray();
        }
        catch
        {
            return null;
        }
    }

    private TokenValidationParameters GetValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("randomJwtToken")!))
        };
    }
}