using Fluxor;

namespace Elevators.Store.Guilds;

[FeatureState]
public class GuildsState
{
    public List<Guild> Guilds { get; }
    public bool IsLoading { get; }

    private GuildsState() {} // Required for creating initial state

    public GuildsState(List<Guild> guilds, bool isLoading)
    {
        Guilds = guilds;
        IsLoading = isLoading;
    }
}
