using Elevators.Store.SelectedGuild.Actions;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace Elevators.Web.Components.DashboardGuildComponents;

public partial class DashboardGuildNoGame
{
    [Inject]
    public ApiClient ApiClient { get; set; } = null!;

    [Parameter]
    public ulong GuildId { get; set; }
    
    [Inject]
    public IDispatcher Dispatcher { get; set; } = null!;
    
    private void HandleCreateGame()
    {
        Dispatcher.Dispatch(new CreateLobbyAction(GuildId));
    }
}