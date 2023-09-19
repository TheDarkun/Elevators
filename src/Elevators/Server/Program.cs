using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (Environment.GetEnvironmentVariable("PROFILE") == "RunDiscord")
{
    // Start the Discord project as a separate process
    Process discordProcess = new Process();
    discordProcess.StartInfo.FileName = "dotnet";
    discordProcess.StartInfo.Arguments = "run --project ../Discord/Discord.csproj";
    discordProcess.StartInfo.WorkingDirectory = "../Discord";
    discordProcess.Start();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();