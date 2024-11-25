using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.AsyncEvents;
using DSharpPlus.Commands;
using DSharpPlus.Net;
using DSharpPlus.Exceptions;
using Elevators.Api.Database;
using Elevators.Api.Discord.Commands;
using Microsoft.EntityFrameworkCore;

// using DSharpPlus.SlashCommands;
// using Elevators.Api.Discord.SlashCommands;

namespace Elevators.Api.Discord;

public class DiscordBot
{
    public DiscordClient Client { get; private set; }
    public async Task InitializeAsync(string token)
    {
        DiscordClientBuilder builder = DiscordClientBuilder.CreateDefault(token, DiscordIntents.All);

        builder.ConfigureServices(services =>
        {
            services.AddDbContext<AppDbContext>(optionsBuilder => optionsBuilder.UseSqlite("Data Source=sqlite.db"));
        });
        builder.UseCommands((IServiceProvider eserviceProvider, CommandsExtension extension) =>
        {
            extension.AddCommands([typeof(JoinCommand), typeof(LeaveCommand)]);
        });
        
        builder.ConfigureEventHandlers
            (
                b => b.HandleMessageCreated(async (s, e) =>
                {
                    if (e.Message.Content.ToLower().StartsWith("ping"))
                    {
                        await e.Message.RespondAsync("pong!");
                    }
                })
            );

        
        Client = builder.Build();

        await Client.ConnectAsync();
    }
}