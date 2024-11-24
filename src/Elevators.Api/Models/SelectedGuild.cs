using System.ComponentModel.DataAnnotations;

namespace Elevators.Api.Models;

public class SelectedGuild
{
    [Key]
    public int Id { get; set; }
    
    public long GuildId { get; set; }
    
    public int PlayedGames { get; set; }
    public List<long> JoinedUsers { get; set; }
    public GuildStatus Status { get; set; }
    
    public long GameRoomId { get; set; }
    public int TopFloor { get; set; }
}

public enum GuildStatus
{
    NoBot,
    NoGame,
    Lobby,
    Game
}