using System.ComponentModel.DataAnnotations;

namespace Elevators.Api.Models;

public class Game
{
    [Key]
    public int Id { get; set; }
    public ulong GuildId { get; set; }
    public List<ulong> JoinedUsers { get; set; }
    public int TopFloor { get; set; }
    public ulong SelectOptionMessageId { get; set; }
    public ulong GameRoomId { get; set; }
    public bool Finished { get; set; } = false;
    public ulong[] WinnerIds { get; set; } = [];
}