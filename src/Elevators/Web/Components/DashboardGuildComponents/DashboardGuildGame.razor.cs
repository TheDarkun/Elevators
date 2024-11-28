using Elevators.Store.Game;
using Elevators.Store.Game.Actions;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace Elevators.Web.Components.DashboardGuildComponents;

public partial class DashboardGuildGame : IDisposable
{
    private System.Timers.Timer _timer;
    
    [Inject]
    public ApiClient ApiClient { get; set; } = null!;
    
    [Inject]
    public IDispatcher Dispatcher { get; set; } = null!;

    [Inject]
    public IState<GameState> GameState { get; set; } = null!;
    
    [Parameter]
    public ulong GuildId { get; set; }
    
    private void HandleDeleteGame()
    {
        _timer?.Stop();
        _timer?.Dispose();
        Dispatcher.Dispatch(new DeleteGameAction(GuildId));
    }

    public async Task HandleSendResults()
    {
        try
        {
            await ApiClient.SendRoundResultsAsync(GuildId);
        }
        catch (Exception _)
        {
        }
    }

    public async Task HandleNextRound()
    {
        try
        {
            await ApiClient.StartNewRoundAsync(GuildId);
        }
        catch (Exception _)
        {
        }
    }
    
    protected override void OnInitialized()
    {
        base.OnInitialized();
        Dispatcher.Dispatch(new FetchCurrentRoundAction(GuildId));
        
        _timer = new (1500);
        _timer.Elapsed += async (sender, e) => Dispatcher.Dispatch(new UpdateGameAction(GuildId));
        _timer.AutoReset = true;
        _timer.Enabled = true;
    }
    
    public void Dispose()
    {
        _timer?.Stop();
        _timer?.Dispose();
    }
}