using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Shared.Models;

namespace Web;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private ClaimsPrincipal anonymous = new ClaimsPrincipal(new ClaimsIdentity());
    
    private IJSRuntime js { get; }
    
    public CustomAuthenticationStateProvider(IJSRuntime js)
    {
        this.js = js;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        ClaimsIdentity identity;

        var token = await js.InvokeAsync<string>("getCookie", "account");

        if (!string.IsNullOrEmpty(token))
        {
            try
            {
                identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            }
            catch (Exception e)
            {
                identity = new ClaimsIdentity();
            }
        }
        else
        {
            identity = new ClaimsIdentity();
        }

        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }

    public async Task<DiscordUser?> GetEncryptedToken()
    {
        throw new NotImplementedException();
    }
    public async Task<string?> GetToken()
    {
        throw new NotImplementedException();
    }
    #region https://github.com/SteveSandersonMS/presentation-2019-06-NDCOslo/blob/master/demos/MissionControl/MissionControl.Client/Util/ServiceExtensions.cs
    
    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
    #endregion
}