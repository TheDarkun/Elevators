using Elevators.State;

namespace Elevators.Components.Layout;

public partial class Header
{
    [Inject] 
    public IConfiguration Configuration { get; set; } = null!;
    
    [CascadingParameter] 
    public AccountState AccountState { get; set; } = null!;
}