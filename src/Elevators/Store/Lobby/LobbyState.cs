using Fluxor;

namespace Elevators.Store.Lobby;

[FeatureState]
public class LobbyState
{
    public List<LobbyUser> Users { get; } = new();
    public Dictionary<string, string> Channels { get; } = new();
    public int TopFloor { get; } = 3;
    public ulong GameRoomId { get; }
    public bool IsLoading { get; }
    
    private LobbyState(){} // Required for creating initial state

    public LobbyState(List<LobbyUser> users, Dictionary<string, string> channels, ulong gameRoomId, bool isLoading, int topFloor)
    {
        Users = users;
        Channels = channels;
        TopFloor = topFloor;
        GameRoomId = gameRoomId;
        IsLoading = isLoading;
    }
}