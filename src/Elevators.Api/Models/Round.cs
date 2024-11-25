using System.ComponentModel.DataAnnotations;

namespace Elevators.Api.Models;

public class Round
{
    [Key]
    public int Id { get; set; }
    public ulong GuildId { get; set; }
    public int CurrentRound { get; set; }
    public int TopFloor { get; set; }
    public List<Player> Players { get; set; } = new();
}

public class Player
{
    [Key]
    public int Id { get; set; }
    public ulong UserId { get; set; }
    public Elevators.PlayerAction PlayerAction { get; set; }
    public ulong? CutPlayerId { get; set; }
    public bool IsAlive { get; set; }
    public int Floor { get; set; }
    
    public int RoundId { get; set; }
}

public enum PlayerAction
{
    NoAction = default,
    Up,
    Idle,
    Cut
}