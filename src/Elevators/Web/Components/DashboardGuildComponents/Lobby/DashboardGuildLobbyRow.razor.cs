using Microsoft.AspNetCore.Components;

namespace Elevators.Web.Components.DashboardGuildComponents.Lobby;

public partial class DashboardGuildLobbyRow
{
    [Parameter]
    public LobbyUser LobbyUser { get; set; } = null!;
}