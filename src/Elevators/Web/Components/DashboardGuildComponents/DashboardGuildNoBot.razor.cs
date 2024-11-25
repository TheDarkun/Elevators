using Microsoft.AspNetCore.Components;

namespace Elevators.Web.Components.DashboardGuildComponents;

public partial class DashboardGuildNoBot
{
    [Inject]
    public ApiClient ApiClient { get; set; } = null!;
    
    [Parameter]
    public ulong GuildId { get; set; }
    
    public string? InviteLink { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        var result = await ApiClient.GetConfigurationInviteLinkAsync(GuildId);
        InviteLink = result.InviteLink;
    }
}