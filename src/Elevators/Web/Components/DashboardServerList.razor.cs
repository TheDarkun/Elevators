using Elevators.State;
using Microsoft.AspNetCore.Components;

namespace Elevators.Web.Components;

public partial class DashboardServerList
{
    
    [Inject] 
    public IConfiguration Configuration { get; set; } = null!;
    
    
    private bool _open;

    private void ToggleOpen() => _open = !_open;
    

}