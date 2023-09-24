using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.JSInterop;
using Shared.Models;

namespace Web.Connectors;

public interface IServerListConnector
{
    public DiscordServer[]? DiscordServers { get; }
    Task RetrieveDiscordServersAsync();
}

public class ServerListConnector : IServerListConnector
{
    private DiscordServer[]? discordServers { get; set; }

    public DiscordServer[]? DiscordServers => discordServers;

    private HttpClient client { get; }
    private IJSRuntime js { get; }

    public ServerListConnector(HttpClient client, IJSRuntime js)
    {
        this.client = client;
        this.js = js;
    }
    
    public async Task RetrieveDiscordServersAsync()
    {
        try
        {
            if (discordServers is not null)
                return;
            
            var token = await js.InvokeAsync<string>("getCookie", "account");

            client.DefaultRequestHeaders.Clear();
        
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync("api/Account/GetJoinedServers");
            if (!response.IsSuccessStatusCode)
                discordServers = null;
        
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            };
            
            var jsonContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<DiscordServer[]>(jsonContent, options);

            if (result is null)
                discordServers =  null;

            discordServers = result;
        }
        catch (Exception)
        {
            discordServers = null;
        }
        
    }
}