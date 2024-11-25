using Elevators.Store.Guilds;
using Elevators.Store.SelectedGuild;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace Elevators.Web.Components.DashboardGuildComponents;

public partial class DashboardGuildAside
{
    [Inject]
    public IState<GuildListState> GuildListState { get; set; } = null!;
    
    [Inject]
    public IState<SelectedGuildState> SelectedGuildState { get; set; } = null!;
    
    [Parameter]
    public ulong GuildId { get; set; }
}