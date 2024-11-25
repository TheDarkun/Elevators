using Elevators.Store.Game;
using Elevators.Store.Game.Actions;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace Elevators.Web.Components.DashboardGuildComponents;

public partial class DashboardGuildGame
{
    [Inject]
    public IDispatcher Dispatcher { get; set; } = null!;

    [Inject]
    public IState<GameState> GameState { get; set; } = null!;
    
    [Parameter]
    public ulong GuildId { get; set; }
    
    private void HandleDeleteGame()
    {
        Dispatcher.Dispatch(new DeleteGameAction(GuildId));
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Dispatcher.Dispatch(new FetchCurrentRoundAction(GuildId));
    }
}