namespace Elevators.Api.Endpoints.Games.Requests;

public class CreateGameRequest
{
    public ulong GuildId { get; set; }
    public ulong GameRoomId { get; set; }
    public int TopFloor { get; set; }
}