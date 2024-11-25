using System.Text;
using Elevators.Api.Database;
using Elevators.Api.Discord;
using Elevators.Authentication;
using FastEndpoints;
using FastEndpoints.ClientGen;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NJsonSchema.CodeGeneration.CSharp;
using Tailwind;

var bld = WebApplication.CreateBuilder(args);
bld.Services.AddCascadingAuthenticationState();
bld.Services.AddDbContext<AppDbContext>(optionsBuilder => optionsBuilder.UseSqlite("Data Source=sqlite.db"));
bld.Services.AddSingleton<DiscordBot>();
bld.Services
    .AddHttpClient()
    .AddFastEndpoints()
    .AddAuthorization()
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddCookie()
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(bld.Configuration.GetValue<string>("randomJwtToken")!))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.TryGetValue("account", out string token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            }
        };
    });

if (bld.Environment.IsDevelopment())
{
    bld.Services.SwaggerDocument(
        o =>
        {
            o.DocumentSettings = d => d.DocumentName = "MyApi";
            o.ShortSchemaNames = true;
            o.RemoveEmptyRequestSchema = true;
        });
}

var app = bld.Build();

if (app.Environment.IsDevelopment())
    app.UseWebAssemblyDebugging();

app.UseBlazorFrameworkFiles()
    .UseStaticFiles();
app.MapFallbackToFile("index.html");
app.UseAuthentication()
    .UseAuthorization()
    .UseFastEndpoints(
        c =>
        {
            c.Endpoints.ShortNames = true;
            c.Serializer.Options.PropertyNamingPolicy = null;
            c.Endpoints.RoutePrefix = "api";
        });

if (app.Environment.IsDevelopment())
    app.UseSwaggerGen();

await app.GenerateClientsAndExitAsync(
    documentName: "MyApi",
    destinationPath: "../Elevators/HttpClient",
    csSettings: c =>
    {
        c.ClassName = "ApiClient";
        c.CSharpGeneratorSettings.Namespace = "Elevators";
        c.CSharpGeneratorSettings.JsonLibrary = CSharpJsonLibrary.SystemTextJson;
    },
    tsSettings: null);

if (app.Environment.IsDevelopment())
{
    _ = app.RunTailwind("tailwind", "../Elevators");
}
var discordBot = app.Services.GetRequiredService<DiscordBot>();
await discordBot.InitializeAsync(bld.Configuration.GetValue<string>("botToken")!);
app.UseMiddleware<AuthenticationMiddleware>();
app.Run();