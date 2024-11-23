using Elevators.Store.Guilds.Actions;
using Fluxor;

namespace Elevators.Store.Guilds;

public class GuildsReducers
{
    [ReducerMethod]
    public static GuildsState ReduceFetchGuildsAction(GuildsState guildsState, FetchGuildsAction action)
        => new GuildsState([], true);
    
    [ReducerMethod]
    public static GuildsState ReduceFetchGuildsResultAction(GuildsState guildsState, FetchGuildsActionResult action)
        => new GuildsState(action.Response.Guilds.ToList(), false);
}