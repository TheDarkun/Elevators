using System.Net;
using MySqlConnector;
using Shared.Models;
using System.Text.Json;
using Server.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Server.Managers.Dashboard;

public class DashboardManager : IDashboardManager
{
    private MySqlConnection Connection { get; }
    private IConfiguration Config { get; }
    private HttpClient Client { get; }
    
    public DashboardManager(MySqlConnection connection, IConfiguration config, HttpClient client)
    {
        Connection = connection;
        Config = config;
        Client = client;
    }

    public async Task<ManagerResult> GetGuildChannels(long guildId)
    {
        try
        {
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Add("Authorization", $"Bot {Config.GetSection("Discord:BotToken").Value!}");
            var response = await Client.GetAsync($"https://discord.com/api/guilds/{guildId}/channels");

            if (!response.IsSuccessStatusCode)
                return new(HttpStatusCode.InternalServerError, response.Content);
        
            var jsonContent = await response.Content.ReadAsStringAsync();
        
            // Deserialize result to an Immutable Array of Guilds
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            IEnumerable<DiscordChannel> guilds =
                JsonSerializer.Deserialize<IEnumerable<DiscordChannel>>(jsonContent, options) ?? Array.Empty<DiscordChannel>();
        
            return new(HttpStatusCode.OK, guilds);
        }
        catch (Exception e)
        {
            return new(HttpStatusCode.InternalServerError, e);
        }
    }
    public async Task<ManagerResult> BotIsJoined(long guildId)
    {
        try
        {
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Add("Authorization", $"Bot {Config.GetSection("Discord:BotToken").Value!}");
        
            var response = await Client.GetAsync($"https://discord.com/api/guilds/{guildId}");

            // 200 - Bot is on the server
            // 404 - Bot is NOT on the server
            if (response.IsSuccessStatusCode)
                return new (HttpStatusCode.OK);

            return new(HttpStatusCode.NotFound);
        }
        catch (Exception e)
        {
            return new(HttpStatusCode.InternalServerError, e);
        }
    }

    public async Task<ManagerResult> GetJoinedGuilds(string id)
    {
        try
        {
            // Get discord token from jwtToken
            var token = await GetUserTokenFromDatabase(id);

            if (token is null)
                return new(HttpStatusCode.InternalServerError, "token is null");
            
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = await Client.GetAsync("https://discord.com/api/users/@me/guilds");

            if (!response.IsSuccessStatusCode)
                return new(response.StatusCode, response.Content);
            
            var jsonContent = await response.Content.ReadAsStringAsync();

            // Deserialize result to an Immutable Array of Guilds
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            // Get only servers, where the user is either an owner or has admin permissions
            // 2147483647 is code for admin powers
            IEnumerable<DiscordServer> guilds =
                JsonSerializer.Deserialize<IEnumerable<DiscordServer>>(jsonContent, options)!.Where(server =>
                    server.Permissions == 2147483647);
            
            return new(HttpStatusCode.OK, guilds);
        }
        catch (Exception e)
        {
            return new(HttpStatusCode.InternalServerError, e);
        }
    }
    
    private async Task<string?> GetUserTokenFromDatabase(string id)
    {
        try
        {
            await Connection.OpenAsync();

            await using var command = new MySqlCommand($"SELECT access_token from users WHERE user_id = '{id}'", Connection);
            var result = await command.ExecuteScalarAsync();

            if (result is null)
                return null;

            var token = result.ToString();
            return token;
        }
        catch (Exception e)
        {
            throw new(e.Message);
        }
        finally
        {
            await Connection.CloseAsync();
        }
    }
}