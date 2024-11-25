using Elevators.Store.Lobby.Actions;
using Fluxor;

namespace Elevators.Store.Lobby;

public class LobbyReducers
{
    [ReducerMethod]
    public static LobbyState ReduceFetchLobbyAction(LobbyState lobbyState, FetchLobbyAction action)
        => new LobbyState(lobbyState.Users, lobbyState.Channels, lobbyState.GameRoomId, true, lobbyState.TopFloor);
    
    [ReducerMethod]
    public static LobbyState ReduceFetchLobbyResultAction(LobbyState lobbyState, FetchLobbyResultAction action)
        => new LobbyState(action.Response.Users.ToList(), action.Response.Channels.ToDictionary(kvp => kvp.Key.ToString(), kvp => kvp.Value.ToString()), lobbyState.GameRoomId, false, lobbyState.TopFloor);
}