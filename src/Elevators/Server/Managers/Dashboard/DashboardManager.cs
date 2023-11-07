using MySqlConnector;
using Shared.Models;
using System.Text.Json;
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

    public async Task<IEnumerable<DiscordChannel>> GetGuildChannels(long guildId)
    {
        Client.DefaultRequestHeaders.Clear();
        Client.DefaultRequestHeaders.Add("Authorization", $"Bot {Config.GetSection("Discord:BotToken").Value!}");
        var response = await Client.GetAsync($"https://discord.com/api/guilds/{guildId}/channels");
        
        var jsonContent = await response.Content.ReadAsStringAsync();
        
        // Deserialize result to an Immutable Array of Guilds
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        IEnumerable<DiscordChannel> result =
            JsonSerializer.Deserialize<IEnumerable<DiscordChannel>>(jsonContent, options) ?? Array.Empty<DiscordChannel>();
        
        return result;
    }
    public async Task<bool> BotIsJoined(long guildId)
    {
        Client.DefaultRequestHeaders.Clear();
        Client.DefaultRequestHeaders.Add("Authorization", $"Bot {Config.GetSection("Discord:BotToken").Value!}");
        
        var response = await Client.GetAsync($"https://discord.com/api/guilds/{guildId}");

        // 200 - Bot is on the server
        // 404 - Bot is NOT on the server
        if (response.IsSuccessStatusCode)
            return true;

        return false;
    }

    public async Task<IEnumerable<DiscordServer>> GetJoinedGuilds(string id)
    {
        // Get discord token from jwtToken
        var token = await GetUserTokenFromDatabase(id);

        if (token is null)
            throw new();
            
        Client.DefaultRequestHeaders.Clear();
        Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var response = await Client.GetAsync("https://discord.com/api/users/@me/guilds");

        var jsonContent = await response.Content.ReadAsStringAsync();

        // Deserialize result to an Immutable Array of Guilds
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
            
        // Get only servers, where the user is either an owner or has admin permissions
        IEnumerable<DiscordServer> result =
            JsonSerializer.Deserialize<IEnumerable<DiscordServer>>(jsonContent, options)!.Where(server =>
                server.Permissions == 2147483647);
            
        return result;
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
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            await Connection.CloseAsync();
        }
    }
}