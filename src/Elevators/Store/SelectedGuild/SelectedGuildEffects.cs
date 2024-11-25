using Elevators.Store.SelectedGuild.Actions;
using Fluxor;

namespace Elevators.Store.SelectedGuild;

public class SelectedGuildEffects(ApiClient apiClient)
{
    [EffectMethod]
    public async Task HandleFetchSelectedGuildAction(FetchSelectedGuildAction action, IDispatcher dispatcher)
    {
        var selectedGuild = await apiClient.GetSelectedGuildAsync(action.GuildId);
        dispatcher.Dispatch(new FetchSelectedGuildResultAction(selectedGuild));
    }
    
    [EffectMethod]
    public async Task HandleCreateLobbyAction(CreateLobbyAction action, IDispatcher dispatcher)
    {
        try
        {
            await apiClient.CreateLobbyAsync(new CreateLobbyRequest(){GuildId = action.GuildId});
        }
        catch (Exception e)
        {
            var selectedGuild = await apiClient.GetSelectedGuildAsync(action.GuildId);
            dispatcher.Dispatch(new FetchSelectedGuildResultAction(selectedGuild));
        }
    }
    
    [EffectMethod]
    public async Task HandleDeleteLobbyAction(DeleteLobbyAction action, IDispatcher dispatcher)
    {
        try
        {
            await apiClient.DeleteLobbyAsync(action.GuildId);
        }
        catch (Exception e)
        {
            var selectedGuild = await apiClient.GetSelectedGuildAsync(action.GuildId);
            dispatcher.Dispatch(new FetchSelectedGuildResultAction(selectedGuild));
        }
    }
}