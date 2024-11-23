using Elevators.Store.Guilds.Actions;
using Fluxor;

namespace Elevators.Store.Guilds;

public class GuildsEffects(ApiClient apiClient)
{
    [EffectMethod]
    public async Task HandleFetchGuildsAction(FetchGuildsAction action, IDispatcher dispatcher)
    {
        var guilds = await apiClient.GetGuildsAsync();
        dispatcher.Dispatch(new FetchGuildsActionResult(guilds));
    }
}