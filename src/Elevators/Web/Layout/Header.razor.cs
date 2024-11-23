using Elevators.State;
using Elevators.Store.Configuration;
using Elevators.Store.Configuration.Actions;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using Dispatcher = Fluxor.Dispatcher;

namespace Elevators.Web.Layout;

public partial class Header
{
    [Inject]
    public IState<ConfigurationState> ConfigurationState { get; set; } = null!;

    [Inject]
    public IDispatcher Dispatcher { get; set; } = null!;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Dispatcher.Dispatch(new FetchConfigurationAction());
    }
    
    
}