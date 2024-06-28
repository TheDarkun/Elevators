namespace Elevators.Models;

public class UserModel(long id, long wins, long loses)
{
    public long Id { get; set; } = id;
    public long Wins { get; set; } = wins;
    public long Loses { get; set; } = loses;
}