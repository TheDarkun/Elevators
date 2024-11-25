using Elevators.Store.Lobby;
using Elevators.Store.Lobby.Actions;
using Elevators.Store.SelectedGuild.Actions;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace Elevators.Web.Components.DashboardGuildComponents;

public partial class DashboardGuildLobby : IDisposable
{
    [Parameter]
    public ulong GuildId { get; set; }

    public string ChannelId { get; set; } = "Select your game room";
    public int FloorCount { get; set; } = 3;
    private System.Timers.Timer _timer;
    
    [Inject]
    public IState<LobbyState> LobbyState { get; set; } = null!;
    
    [Inject]
    public IDispatcher Dispatcher { get; set; } = null!;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Dispatcher.Dispatch(new FetchLobbyAction(GuildId));

        _timer = new (1500);
        _timer.Elapsed += async (sender, e) => Dispatcher.Dispatch(new UpdateLobbyAction(GuildId));
        _timer.AutoReset = true;
        _timer.Enabled = true;
    }

    private void HandleDeleteGame()
    {
        _timer?.Stop();
        _timer?.Dispose();
        Dispatcher.Dispatch(new DeleteLobbyAction(GuildId));
    }
    
    public void Dispose()
    {
        _timer?.Stop();
        _timer?.Dispose();
    }

    public async Task HandleStartGame()
    {
        if (ulong.TryParse(ChannelId, out var id))
        {
            Console.WriteLine(ChannelId);
            Console.WriteLine(FloorCount);
        }
    }
}