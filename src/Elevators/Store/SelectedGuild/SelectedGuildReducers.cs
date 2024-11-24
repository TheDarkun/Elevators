using Elevators.Store.SelectedGuild.Actions;
using Fluxor;

namespace Elevators.Store.SelectedGuild;

public class SelectedGuildReducers
{
    [ReducerMethod]
    public static SelectedGuildState ReduceFetchSelectedGuildState(SelectedGuildState state, FetchSelectedGuildAction action)
        => new SelectedGuildState(GuildStatus.NoBot, true);
    
    [ReducerMethod]
    public static SelectedGuildState ReduceFetchSelectedGuildResultState(SelectedGuildState state, FetchSelectedGuildResultAction action)
        => new SelectedGuildState(action.Response.SelectedGuild.Status, false);
}