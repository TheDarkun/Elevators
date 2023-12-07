using Discord.Commands;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;

var discordConfig = new DiscordConfiguration
{
    Intents = DiscordIntents.All,
    Token = args[0],
    TokenType = TokenType.Bot,
    AutoReconnect = true
};

var client = new DiscordClient(discordConfig);

var slashCommandsConfig = client.UseSlashCommands();

slashCommandsConfig.RegisterCommands<DebugCommands>();

// client.Ready += ClientReady;

await client.ConnectAsync();
await Task.Delay(-1);

Task ClientReady(DiscordClient sender, ReadyEventArgs args)
{
    return Task.CompletedTask;
}