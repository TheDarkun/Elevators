using Elevators.State;
using Elevators.Store.Guilds;
using Elevators.Store.Guilds.Actions;
using Elevators.Store.User;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace Elevators.Web.Components;

public partial class DashboardServerList
{
    [Inject]
    public IState<UserState> UserState { get; set; } = null!;
    
    [Inject]
    public IState<GuildListState> GuildState { get; set; } = null!;

    [Inject]
    public IDispatcher Dispatcher { get; set; } = null!;
    protected override void OnInitialized()
    {
        base.OnInitialized();
        if (GuildState.Value.Guilds == null)
            Dispatcher.Dispatch(new FetchGuildListAction());
    }

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;
    
    private async Task HandleGuildClick(long guildId)
    {
        if (guildId == 0)
        {
            NavigationManager.NavigateTo("/dashboard");
        }
        else
        {
            NavigationManager.NavigateTo($"/dashboard/{guildId}");
        }
    }
}