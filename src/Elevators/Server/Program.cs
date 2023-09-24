using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using Server.Controllers.Account;
using Server.Managers.Account;
using Tailwind;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddHttpClient();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("Auth:Token").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddSingleton<IAccountManager, AccountManager>();
builder.Services.AddSingleton<IAccountController, AccountController>();

builder.Services.AddTransient<MySqlConnection>(_ =>
    new MySqlConnection(builder.Configuration.GetConnectionString("Main")));

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.RunTailwind("dev", "../Web");
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
    discordProcess.StartInfo.Arguments = "run --project ../Discord/Discord.csproj ";

    discordProcess.StartInfo.Arguments += builder.Configuration.GetSection("Discord:BotToken").Value;
    
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

app.UseAuthentication();
app.UseAuthorization();

app.Run();