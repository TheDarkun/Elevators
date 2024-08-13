using Elevators.State;
    
namespace Elevators.Web.Components;

public partial class DashboardUserInfo
{
    
    [Inject] 
    public IConfiguration Configuration { get; set; } = null!;
    
    [CascadingParameter] 
    public AccountState AccountState { get; set; } = null!;
    
    private bool _open;

    private void ToggleOpen() => _open = !_open;
    
}