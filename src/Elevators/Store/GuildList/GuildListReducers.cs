using Elevators.Store.Guilds.Actions;
using Fluxor;

namespace Elevators.Store.Guilds;

public class GuildListReducers
{
    [ReducerMethod]
    public static GuildListState ReduceFetchGuildsAction(GuildListState guildListState, FetchGuildListAction action)
        => new GuildListState([], true);
    
    [ReducerMethod]
    public static GuildListState ReduceFetchGuildsResultAction(GuildListState guildListState, FetchGuildlistActionResult action)
        => new GuildListState(action.Response.Guilds.ToList(), false);
}