using Elevators;
// using Elevators.Authentication;
using Elevators.Web;
using Fluxor;
using Fluxor.Blazor.Web.ReduxDevTools;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddSingleton(
    new ApiClient(
        builder.HostEnvironment.BaseAddress,
        new HttpClient() { BaseAddress = new(builder.HostEnvironment.BaseAddress) }));
builder.Services.AddMudServices();
builder.Services.AddMudBlazorDialog();

var currentAssembly = typeof(Program).Assembly;
builder.Services.AddFluxor(options =>
{
    options.ScanAssemblies(currentAssembly);
    options.UseReduxDevTools();
});
// builder.Services.AddOptions();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();