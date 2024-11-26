using AsciiTableFormatter;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Elevators.Api.Database;
using Elevators.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Elevators.Api.Discord;

public class ElevatorsManager(AppDbContext appDbContext, DiscordBot discordBot)
{
    public async Task ShowGameStatus(ulong guildId)
    {
        var game = await appDbContext.Games.FirstOrDefaultAsync(g => g.GuildId == guildId);
        var guild = discordBot.Client.Guilds.FirstOrDefault(g => g.Key == guildId).Value;
        var channel = guild.Channels.FirstOrDefault(c => c.Key == game.GameRoomId).Value;


        var table = new List<GameStatus>();
        var currentRound = appDbContext.Rounds.FirstOrDefault(r => r.GuildId == guildId).CurrentRound;
        var players = appDbContext.Rounds.FirstOrDefault(r => r.GuildId == guildId).Players;
        
        foreach (var player in players)
        {
            table.Add(new GameStatus()
            {
                Eliminated = "❤️",
                Contestant = guild.Members.FirstOrDefault(m => m.Key == player.UserId).Value.Username,
                Floor = 1,
                Action = "Pending..."
            });
        }

        string output = Formatter.Format(table);
        var embed = new DiscordEmbedBuilder
        {
            Title = "Initial game state",
            
            Description = $"""
                           **Top Floor:** {game.TopFloor}
                           **Current Round:** {currentRound}
                           ```
                           {output}
                           ```
                           """, 
            Color = DiscordColor.Blurple
        };
        var dropdownOptions = new List<DiscordSelectComponentOption>
        {
            new DiscordSelectComponentOption("Up", "up"),
            new DiscordSelectComponentOption("Idle", "idle"),
        };

        foreach (var player in players)
        {
            var name = guild.Members.FirstOrDefault(m => m.Key == player.UserId).Value.Username;
            dropdownOptions.Add(new DiscordSelectComponentOption($"Cut {name}", player.UserId.ToString()));
        }
        var dropdown = new DiscordSelectComponent("dropdown1", "Select your action", dropdownOptions);
        // await channel.SendMessageAsync(new DiscordMessageBuilder().WithContent("ahoj").AddComponents(dropdown));
        var messageBuilder = new DiscordMessageBuilder()
            .AddEmbed(embed)
            .AddComponents(dropdown);
        await channel.SendMessageAsync(messageBuilder);
        // await channel.SendMessageAsync(embed: embed);
    }

    public async Task ShowGameActionSelect(ulong guildId)
    {
        var game = await appDbContext.Games.FirstOrDefaultAsync(g => g.GuildId == guildId);
        var guild = discordBot.Client.Guilds.FirstOrDefault(g => g.Key == guildId).Value;
        var channel = guild.Channels.FirstOrDefault(c => c.Key == game.GameRoomId).Value;


        var table = new List<GameStatus>();
        var currentRound = appDbContext.Rounds.FirstOrDefault(r => r.GuildId == guildId).CurrentRound;
        var players = appDbContext.Rounds.FirstOrDefault(r => r.GuildId == guildId).Players;
        
        var dropdownOptions = new List<DiscordSelectComponentOption>
        {
            new DiscordSelectComponentOption("Up", "up"),
            new DiscordSelectComponentOption("Idle", "idle"),
        };

        foreach (var player in players)
        {
            var name = guild.Members.FirstOrDefault(m => m.Key == player.UserId).Value.Username;
            dropdownOptions.Add(new DiscordSelectComponentOption($"Cut {name}", player.UserId.ToString()));
        }
        var dropdown = new DiscordSelectComponent("dropdown1", "Select your action", dropdownOptions);
        await channel.SendMessageAsync(new DiscordMessageBuilder().WithContent("ahoj").AddComponents(dropdown));
    }

    public async Task HandlePlayerActionSelect(ComponentInteractionCreatedEventArgs e)
    {
        
        
        var round = await appDbContext.Rounds.Include(round => round.Players).FirstOrDefaultAsync(r => r.GuildId == e.Guild.Id);
        var player = round.Players.FirstOrDefault(p => p.UserId == e.User.Id);

        if (e.Interaction.Data.Values[0] == "up")
        {
            player.PlayerAction = PlayerAction.Up;
        }
        else if (e.Interaction.Data.Values[0] == "idle")
        {
            player.PlayerAction = PlayerAction.Idle;
        }
        else
        {
            player.PlayerAction = PlayerAction.Cut;
            player.CutPlayerId = ulong.Parse(e.Interaction.Data.Values[0]);
        }

        await appDbContext.SaveChangesAsync();
        
        await e.Interaction.CreateResponseAsync(DiscordInteractionResponseType.ChannelMessageWithSource, 
            new DSharpPlus.Entities.DiscordInteractionResponseBuilder()
                .WithContent($"{e.User.Username} chose {e.Interaction.Data.Values[0]}")
                .AsEphemeral());

    }
    public async Task ShowGameResult()
    {
        
    }
}