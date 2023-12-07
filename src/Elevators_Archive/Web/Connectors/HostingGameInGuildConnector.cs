using System.Net.Http.Headers;
using Microsoft.JSInterop;

namespace Web.Connectors;

public interface IHostingGameOnServerConnector
{
    public List<long> NotJoinedServers { get; }
    public List<long> JoinedServers { get; }
    public Task BotIsJoined(long serverId);
} 

public class HostingGameInGuildConnector : IHostingGameOnServerConnector
{
    private HttpClient client { get; }
    private IJSRuntime js { get; }

    
    public HostingGameInGuildConnector(HttpClient client, IJSRuntime js)
    {
        this.client = client;
        this.js = js;

        NotJoinedServers = new();
        JoinedServers = new();
    }

    public List<long> NotJoinedServers { get; }
    public List<long> JoinedServers { get; }

    public async Task BotIsJoined(long serverId)
    {
        try
        {
            if (NotJoinedServers.Contains(serverId) || JoinedServers.Contains(serverId))
                return;
            
            var token = await js.InvokeAsync<string>("getCookie", "account");

            client.DefaultRequestHeaders.Clear();
        
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync($"api/Dashboard/BotIsJoined/{serverId}");

            if (response.IsSuccessStatusCode)
            {
                JoinedServers.Add(serverId);
            }
            else
            {
                NotJoinedServers.Add(serverId);
            }
        }
        catch (Exception)
        {
            // ignored
        }
    }
}