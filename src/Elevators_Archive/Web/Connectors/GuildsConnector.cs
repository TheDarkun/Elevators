using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.JSInterop;
using Shared.Models;

namespace Web.Connectors;

public interface IServerListConnector
{
    public DiscordServer[]? Guilds { get; }
    Task RetrieveGuildsAsync();
}

public class GuildsConnector : IServerListConnector
{
    private DiscordServer[]? guilds { get; set; }

    public DiscordServer[]? Guilds => guilds;

    private HttpClient Client { get; }
    private IJSRuntime Js { get; }

    public GuildsConnector(HttpClient client, IJSRuntime js)
    {
        Client = client;
        Js = js;
    }
    
    public async Task RetrieveGuildsAsync()
    {
        try
        {
            if (guilds is not null)
                return;
            
            var token = await Js.InvokeAsync<string>("getCookie", "account");

            Client.DefaultRequestHeaders.Clear();
        
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.GetAsync("api/Dashboard/GetJoinedGuilds");
            if (!response.IsSuccessStatusCode)
                guilds = null;
        
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            };
            
            var jsonContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<DiscordServer[]>(jsonContent, options);

            if (result is null)
                guilds = null;

            guilds = result;
        }
        catch (Exception)
        {
            guilds = null;
        }
        
    }
}