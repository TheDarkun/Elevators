using Microsoft.AspNetCore.Components;

namespace Elevators.Web;

public partial class LogoutButton
{
    [Inject]
    public ApiClient ApiClient { get; set; } = null!;
    
    private async Task Logout()
    {
        try
        {
            await ApiClient.GetUserLogoutAsync();
        }
        catch (Exception)
        {
            Navigation.NavigateTo("/", forceLoad: true);
        }
    }
}