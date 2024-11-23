using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Elevators;

public class JwtAuthenticationStateProvider(ApiClient apiClient) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var response = await apiClient.GetAccountTokenAsync();
            
            if (response.Token is null)
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(response.Token);

            var identity = new ClaimsIdentity(jwt.Claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }
        catch (Exception)
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }
}