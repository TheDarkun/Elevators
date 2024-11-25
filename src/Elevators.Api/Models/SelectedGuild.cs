using System.ComponentModel.DataAnnotations;

namespace Elevators.Api.Models;

public class SelectedGuild
{
    [Key]
    public int Id { get; set; }
    
    public ulong GuildId { get; set; }
    public int PlayedGames { get; set; }
    public GuildStatus Status { get; set; }
    
}

public enum GuildStatus
{
    NoBot,
    NoGame,
    Lobby,
    Game
}