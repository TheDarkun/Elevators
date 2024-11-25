using Elevators.Store.SelectedGuild;
using Elevators.Store.SelectedGuild.Actions;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace Elevators.Web.Components;

public partial class DashboardGuild
{
    [Inject]
    public IState<SelectedGuildState> SelectedGuildState { get; set; } = null!;
    
    [Inject]
    public IDispatcher Dispatcher { get; set; } = null!;
    
    [Parameter]
    public ulong GuildId { get; set; }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        Dispatcher.Dispatch(new FetchSelectedGuildAction(GuildId));
    }
    //
    // protected override void OnInitialized()
    // {
    //     base.OnInitialized();
    //     Dispatcher.Dispatch(new FetchSelectedGuildAction(GuildId));
    // }
}