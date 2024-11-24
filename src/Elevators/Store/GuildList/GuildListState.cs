using Fluxor;

namespace Elevators.Store.Guilds;

[FeatureState]
public class GuildListState
{
    public List<Guild> Guilds { get; }
    public bool IsLoading { get; }

    private GuildListState() {} // Required for creating initial state

    public GuildListState(List<Guild> guilds, bool isLoading)
    {
        Guilds = guilds;
        IsLoading = isLoading;
    }
}
