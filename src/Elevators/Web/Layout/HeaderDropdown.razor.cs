using Elevators.Store.User;
using Elevators.Store.User.Actions;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace Elevators.Web.Layout;

public partial class HeaderDropdown
{
    [Inject]
    public IState<UserState> UserState { get; set; } = null!;
    
    [Inject]
    public IDispatcher Dispatcher { get; set; } = null!;
    
    private bool _open;

    private void ToggleOpen() => _open = !_open;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Dispatcher.Dispatch(new FetchUserAction());
    }
}