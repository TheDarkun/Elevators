using Elevators.Store.Configuration;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace Elevators.Web.Pages;

public partial class Home
{
    [Inject]
    public IState<ConfigurationState> ConfigurationState { get; set; } = null!;
}