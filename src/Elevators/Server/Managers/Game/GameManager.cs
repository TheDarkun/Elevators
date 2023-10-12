using MySqlConnector;
using Shared.Models;

namespace Server.Managers.Game;

public class GameManager : IGameManager
{
    private HttpClient Client { get; }
    private IConfiguration Config { get; }
    private MySqlConnection Connection { get; }
    
    public GameManager(IConfiguration config, HttpClient client, MySqlConnection connection)
    {
        Config = config;
        Client = client;
        Connection = connection;
    }
    
    public async Task CreateGame(GameCreateDTO game)
    {
        try
        {
            await Connection.OpenAsync();

            // Check just for sure if server does not already have a game

            await using var command = new MySqlCommand($"SELECT Count(*) FROM games WHERE 'server_id' == {game.ServerId}", Connection);
            var result = await command.ExecuteScalarAsync();

            if (result is null || (int)result > 0)
                throw new();
            // Create game
            await using var secondCommand = new MySqlCommand($"INSERT INTO games VALUES('{game.ServerId}', '{game.MaxPlayers}')", Connection);
            // await using var command = new MySqlCommand($"INSERT INTO users VALUES ('{discordUserId}', '{accessToken}', '{refreshToken}');", Connection);
            // await command.ExecuteNonQueryAsync();
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