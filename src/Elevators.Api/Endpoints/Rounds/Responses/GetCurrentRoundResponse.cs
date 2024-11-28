using Elevators.Api.Models;

namespace Elevators.Api.Endpoints.Rounds.Responses;

public class GetCurrentRoundResponse
{
    public List<GetCurrentRoundPlayerResponse> Players { get; set; } = new();
    public bool IsFinished { get; set; }
    public int CurrentRound { get; set; }
    public int TopFloor { get; set; }
    public bool Finished { get; set; }
    public ulong[] WinnerIds { get; set; }
}

public class GetCurrentRoundPlayerResponse
{
    public string Name { get; set; }
    public ulong UserId { get; set; }
    public string Avatar { get; set; }
    public bool IsAlive { get; set; }
    public PlayerAction PlayerAction { get; set; }
    public int Floor { get; set; }
    public ulong? CutPlayerId { get; set; }
    public bool Submitted { get; set; }
}
