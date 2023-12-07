using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Web.Connectors;

public interface ILogoutConnector
{
    public Task Logout();
}

public class LogoutConnector : ILogoutConnector
{
    private HttpClient Client { get; }
    private NavigationManager Navigation { get; }
    private IJSRuntime Js { get; }
    
    public LogoutConnector(HttpClient client, NavigationManager navigation, IJSRuntime js)
    {
        Client = client;
        Navigation = navigation;
        Js = js;
    }
    
    public async Task Logout()
    {
        Console.WriteLine("yo");
        var token = await Js.InvokeAsync<string>("getCookie", "account");

        Client.DefaultRequestHeaders.Clear();
        
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); 
        await Client.DeleteAsync("api/Account/Logout");

        await Js.InvokeVoidAsync("removeCookie", "account");
        
        Navigation.NavigateTo("/", true);
    }
}