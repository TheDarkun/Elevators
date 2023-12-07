using System.Diagnostics;
using Elevators.Components;
using Elevators.Controllers;
using Tailwind;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers();

builder.Services.AddTransient<AccountController>();
builder.Services.AddTransient<DashboardController>();
builder.Services.AddTransient<GameController>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.RunTailwind("dev");

}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller=Home}/{action=Index}/{id?}");

app.UseAntiforgery();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Elevators.Client._Imports).Assembly);

if (Environment.GetEnvironmentVariable("PROFILE") == "RunBot")
{
    // Start the Discord project as a separate process
    Process discordProcess = new Process();
    discordProcess.StartInfo.FileName = "dotnet";
    discordProcess.StartInfo.Arguments = "run --project ../Elevators.Bot/Elevators.Bot.csproj ";

    // discordProcess.StartInfo.Arguments += builder.Configuration.GetSection("Discord:BotToken").Value;
    
    discordProcess.StartInfo.WorkingDirectory = "../Elevators.Bot";
    discordProcess.Start();
}

app.Run();

