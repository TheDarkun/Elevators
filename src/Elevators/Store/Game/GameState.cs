using Fluxor;

namespace Elevators.Store.Game;

[FeatureState]
public class GameState
{
    public bool IsLoading { get; }
    public bool IsFinished { get; }
    public int CurrentRound { get; }
    public int TopFloor { get; }
    public List<Player> Players { get; } = [];
    private GameState() {} // Required for creating initial state

    public GameState(bool isLoading, bool isFinished, int currentRound, int topFloor, List<Player> players)
    {
        IsLoading = isLoading;
        IsFinished = isFinished;
        CurrentRound = currentRound;
        TopFloor = topFloor;
        Players = players;
    }
}

public class Player
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