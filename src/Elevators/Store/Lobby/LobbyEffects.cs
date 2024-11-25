using Elevators.Store.Lobby.Actions;
using Fluxor;

namespace Elevators.Store.Lobby;

public class LobbyEffects(ApiClient apiClient)
{
    [EffectMethod]
    public async Task HandleFetchLobbyAction(FetchLobbyAction action, IDispatcher dispatcher)
    {
        var lobby = await apiClient.GetLobbyAsync(action.GuildId);
        dispatcher.Dispatch(new FetchLobbyResultAction(lobby));
    }

    [EffectMethod]
    public async Task HandleUpdateLobbyAction(UpdateLobbyAction action, IDispatcher dispatcher)
    {
        var lobby = await apiClient.GetLobbyAsync(action.GuildId);
        dispatcher.Dispatch(new FetchLobbyResultAction(lobby));
    }
}