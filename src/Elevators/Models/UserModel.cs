namespace Elevators.Models;

public class UserModel(ulong id, long wins, long loses)
{
    public ulong Id { get; set; } = id;
    public long Wins { get; set; } = wins;
    public long Loses { get; set; } = loses;
}