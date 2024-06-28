using Dapper;
using Elevators.Models;
using Microsoft.Data.Sqlite;

namespace Elevators.Database;

public class DataAccess : IDataAccess
{
    private readonly SqliteConnection _sqliteConnection;

    public DataAccess(SqliteConnection sqliteConnection)
    {
        _sqliteConnection = sqliteConnection;

        // Initialize database
        sqliteConnection.Query("""
                               CREATE TABLE IF NOT EXISTS users (
                                   id INTEGER PRIMARY KEY,
                                   wins INTEGER,
                                   loses INTEGER
                               )
                               """);
        
    }

    public async Task CreateOrUpdateUserModelAsync(UserModel userModel)
    {
        var query = """
                    INSERT INTO users (id, wins, loses) 
                    VALUES (@Id, @Wins, @Loses)
                    ON CONFLICT(id) DO UPDATE SET
                    wins = excluded.wins,
                    loses = excluded.loses
                    """;
        await _sqliteConnection.ExecuteAsync(query, userModel);
    }

    public async Task<UserModel?> GetUserModelByIdAsync(ulong id)
    {
        var query = "SELECT * FROM users WHERE id = @Id";
        return await _sqliteConnection.QuerySingleOrDefaultAsync<UserModel>(query, new { Id = id });
    }
}