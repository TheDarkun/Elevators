using Fluxor;

namespace Elevators.Store.SelectedGuild;

[FeatureState]
public class SelectedGuildState
{
    public GuildStatus GuildStatus { get; set; }
    public bool IsLoading { get; }
    public int MemberCount { get; }
    
    private SelectedGuildState() {} // Required for creating initial state

    public SelectedGuildState(GuildStatus guildStatus, int memberCount, bool isLoading)
    {
        GuildStatus = guildStatus;
        IsLoading = isLoading;
        MemberCount = memberCount;
    }
}