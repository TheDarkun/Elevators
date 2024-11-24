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
}