using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.JSInterop;
using Shared.Models;

namespace Web.Connectors;

public interface IChannelsConnector
{
    public Task GetChannels(string guildId);
}

public class ChannelsConnector : IChannelsConnector
{
    private DiscordChannel[]? channels { get; set; }

    public DiscordChannel[]? Channels => channels;
 
    private HttpClient Client { get; }
    private IJSRuntime Js { get; }

    public ChannelsConnector(HttpClient client, IJSRuntime js)
    {
        Client = client;
        Js = js;
    }
    
    public async Task GetChannels(string guildId)
    {
        try
        {
            if (channels is not null)
                return;
            
            var token = await Js.InvokeAsync<string>("getCookie", "account");

            Client.DefaultRequestHeaders.Clear();
        
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.GetAsync("api/Dashboard/GetJoinedGuilds");
            
            if (!response.IsSuccessStatusCode)
                channels = null;
        
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            };
            
            var jsonContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<DiscordChannel[]>(jsonContent, options);

            if (result is null)
                channels = null;

            channels = result;
            
        }
        catch (Exception)
        {
            
        }
    }
}