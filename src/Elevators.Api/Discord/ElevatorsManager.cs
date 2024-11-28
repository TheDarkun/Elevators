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

        var round = await appDbContext.Rounds.Include(round => round.Players).FirstOrDefaultAsync(r => r.GuildId == guildId);

        var table = new List<GameStatus>();
        var currentRound = appDbContext.Rounds.FirstOrDefault(r => r.GuildId == guildId).CurrentRound;
        var players = appDbContext.Rounds.FirstOrDefault(r => r.GuildId == guildId).Players;
        
        foreach (var player in players)
        {
            table.Add(new GameStatus()
            {
                Contestant = $"{(player.IsAlive ? '❤' : '✝')} {guild.Members.FirstOrDefault(m => m.Key == player.UserId).Value.Username}",
                Floor = player.Floor,
                Action = "Pending..."
            });
        }

        string output = Formatter.Format(table);
        var embed = new DiscordEmbedBuilder
        {
            Title = $"Round {round.CurrentRound} State",
            
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
            if (!player.IsAlive)
                continue;
            
            var name = guild.Members.FirstOrDefault(m => m.Key == player.UserId).Value.Username;
            dropdownOptions.Add(new DiscordSelectComponentOption($"Cut {name}", player.UserId.ToString()));
        }
        var dropdown = new DiscordSelectComponent("dropdown1", "Select your action", dropdownOptions);
        // await channel.SendMessageAsync(new DiscordMessageBuilder().WithContent("ahoj").AddComponents(dropdown));
        var messageBuilder = new DiscordMessageBuilder()
            .AddEmbed(embed);
            // .AddComponents(dropdown);
        await channel.SendMessageAsync(messageBuilder);

        var message = await channel.SendMessageAsync(new DiscordMessageBuilder().WithContent("Select your action").AddComponents(dropdown));
        game.SelectOptionMessageId = message.Id;
        await appDbContext.SaveChangesAsync();
        // await appDbContext.Rounds.
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
            if (!player.IsAlive)
                continue;
            
            var name = guild.Members.FirstOrDefault(m => m.Key == player.UserId).Value.Username;
            dropdownOptions.Add(new DiscordSelectComponentOption($"Cut {name}", player.UserId.ToString()));
        }
        var dropdown = new DiscordSelectComponent("dropdown1", "Select your action", dropdownOptions);
        await channel.SendMessageAsync(new DiscordMessageBuilder().WithContent("ahoj").AddComponents(dropdown));
    }

    public async Task HandlePlayerActionSelect(ComponentInteractionCreatedEventArgs e)
    {
        
        
        var round = await appDbContext.Rounds.Include(round => round.Players).FirstOrDefaultAsync(r => r.GuildId == e.Guild.Id);

        if (!round.Players.Any(p => p.UserId == e.User.Id))
        {
            await e.Interaction.CreateResponseAsync(DiscordInteractionResponseType.ChannelMessageWithSource, 
                new DSharpPlus.Entities.DiscordInteractionResponseBuilder()
                    .WithContent("You are not part of this game")
                    .AsEphemeral());
            return;
        }
        
        var submitPlayer = round.Players.FirstOrDefault(p => p.UserId == e.User.Id);
        if (!submitPlayer.IsAlive)
        {
            await e.Interaction.CreateResponseAsync(DiscordInteractionResponseType.ChannelMessageWithSource, 
                new DSharpPlus.Entities.DiscordInteractionResponseBuilder()
                    .WithContent("You can no longer submit actions because you have been eliminated")
                    .AsEphemeral());
            return;
        }
        
        if (e.Interaction.Data.Values[0] == "up")
        {
            submitPlayer.PlayerAction = PlayerAction.Up;
        }
        else if (e.Interaction.Data.Values[0] == "idle")
        {
            submitPlayer.PlayerAction = PlayerAction.Idle;
        }
        else
        {
            submitPlayer.PlayerAction = PlayerAction.Cut;
            submitPlayer.CutPlayerId = ulong.Parse(e.Interaction.Data.Values[0]);
        }

        var lastResponse = !round.Players.Any(p => p.PlayerAction == PlayerAction.NoAction && p.IsAlive);
        Console.WriteLine(lastResponse);
        Console.WriteLine(lastResponse);
        Console.WriteLine(lastResponse);
        Console.WriteLine(lastResponse);
        Console.WriteLine(lastResponse);
        Console.WriteLine(lastResponse);
        Console.WriteLine(lastResponse);
        Console.WriteLine(lastResponse);
        Console.WriteLine(lastResponse);
        Console.WriteLine(lastResponse);
        Console.WriteLine(lastResponse);
        Console.WriteLine(lastResponse);
        Console.WriteLine(lastResponse);
        Console.WriteLine(lastResponse);
        Console.WriteLine(lastResponse);

        if (lastResponse)
        {
            var game = await appDbContext.Games.FirstOrDefaultAsync(g => g.GuildId == e.Guild.Id);
            var channel = discordBot.Client.Guilds.FirstOrDefault(g => g.Key == e.Guild.Id).Value.Channels
                .FirstOrDefault(c => c.Key == game.GameRoomId).Value;
            await channel.DeleteMessageAsync(await channel.GetMessageAsync(game.SelectOptionMessageId, true));

            foreach (var player in round.Players)
            {
                if (player.PlayerAction == PlayerAction.Cut)
                {
                    var target = round.Players.FirstOrDefault(p => p.UserId == player.CutPlayerId);
                    if (target.PlayerAction != PlayerAction.Idle)
                    {
                        target.IsAlive = false;
                    }
                }
            }

            foreach (var player in round.Players)
            {
                if (!player.IsAlive)
                    continue;

                if (player.PlayerAction == PlayerAction.Up)
                {
                    player.Floor++;
                }
            }

            var survivors = round.Players.Where(p => p.IsAlive);

            if (survivors.Count() == 1)
            {
                game.Finished = true;
                game.WinnerIds = [survivors.First().UserId];
            }

            var topFloorers = round.Players.Where(p => p.Floor == game.TopFloor);

            if (topFloorers.Any())
            {
                foreach (var topFloorer in topFloorers)
                {
                    game.Finished = true;
                    game.WinnerIds = topFloorers.Select(p => p.UserId).ToArray();
                }
            }
        }

        await appDbContext.SaveChangesAsync();
        
        await e.Interaction.CreateResponseAsync(DiscordInteractionResponseType.ChannelMessageWithSource, 
            new DSharpPlus.Entities.DiscordInteractionResponseBuilder()
                .WithContent($"{e.User.Username} chose {e.Interaction.Data.Values[0]}")
                .AsEphemeral());

    }
    public async Task ShowGameResult(ulong guildId)
    {
        var game = await appDbContext.Games.FirstOrDefaultAsync(g => g.GuildId == guildId);
        var guild = discordBot.Client.Guilds.FirstOrDefault(g => g.Key == guildId).Value;
        var channel = guild.Channels.FirstOrDefault(c => c.Key == game.GameRoomId).Value;

        var round = await appDbContext.Rounds.Include(round => round.Players).FirstOrDefaultAsync(r => r.GuildId == guildId);

        var table = new List<GameStatus>();
        var currentRound = appDbContext.Rounds.FirstOrDefault(r => r.GuildId == guildId).CurrentRound;
        var players = round.Players;
        
        foreach (var player in players)
        {
            table.Add(new GameStatus()
            {
                Contestant = $"{(player.IsAlive ? '❤' : '✝')} {guild.Members.FirstOrDefault(m => m.Key == player.UserId).Value.Username}",
                Floor = player.Floor,
                Action = $"{(player.IsAlive ? player.PlayerAction : "Eliminated")} {(player.PlayerAction == PlayerAction.Cut ? guild.Members.FirstOrDefault(m => m.Key == player.CutPlayerId).Value.Username : "")}"
            });
        }

        string output = Formatter.Format(table);
        var embed = new DiscordEmbedBuilder
        {
            Title = $"Round {round.CurrentRound} Results",
            
            Description = $"""
                           **Top Floor:** {game.TopFloor}
                           **Current Round:** {currentRound}
                           ```
                           {output}
                           ```
                           """, 
            Color = DiscordColor.Blurple
        };

        var messageBuilder = new DiscordMessageBuilder()
            .AddEmbed(embed);
        await channel.SendMessageAsync(messageBuilder);
    }
}