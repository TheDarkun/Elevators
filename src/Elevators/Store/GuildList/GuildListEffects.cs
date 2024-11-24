using Elevators.Store.Guilds.Actions;
using Fluxor;

namespace Elevators.Store.Guilds;

public class GuildListEffects(ApiClient apiClient)
{
    [EffectMethod]
    public async Task HandleFetchGuildsAction(FetchGuildListAction action, IDispatcher dispatcher)
    {
        var guilds = await apiClient.GetGuildListAsync();
        dispatcher.Dispatch(new FetchGuildlistActionResult(guilds));
    }
}