using AsciiTableFormatter;
using DSharpPlus.Entities;
using Elevators.Api.Database;
using Elevators.Api.Discord;
using Elevators.Api.Endpoints.Games.Requests;
using Elevators.Api.Models;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Elevators.Api.Endpoints.Games;

public class CreateGame : Endpoint<CreateGameRequest>
{
    public AppDbContext AppDbContext { get; set; } = null!;
    public DiscordBot DiscordBot { get; set; } = null!;
    public override void Configure()
    {
        Post("/games/create");
    }

    public override async Task HandleAsync(CreateGameRequest req, CancellationToken ct)
    {
        var game = await AppDbContext.Games.FirstOrDefaultAsync(g => g.GuildId == req.GuildId, ct);
        var selectedGuild = await AppDbContext.SelectedGuilds.FirstOrDefaultAsync(g => g.GuildId == req.GuildId, ct);

        var players = new List<Player>();
        foreach (var joinedUser in game.JoinedUsers)
        {
            var player = new Player()
            {
                Floor = 1,
                IsAlive = true,
                PlayerAction = PlayerAction.NoAction,
                UserId = joinedUser,
            };
            players.Add(player);
        }
        var round = new Round()
        {
            GuildId = req.GuildId,
            Players = players,
            CurrentRound = 1
        };
        game.TopFloor = req.TopFloor;
        game.GameRoomId = req.GameRoomId;
        AppDbContext.Rounds.Add(round);
        selectedGuild.Status = Models.GuildStatus.Game;
        await AppDbContext.SaveChangesAsync(ct);
        await SendOkAsync(null, ct);

        var guild = DiscordBot.Client.Guilds.FirstOrDefault(g => g.Key == req.GuildId).Value;
        var channel = guild.Channels.FirstOrDefault(c => c.Key == req.GameRoomId).Value;


        var table = new List<TestSix>();

        foreach (var player in players)
        {
            table.Add(new TestSix()
            {
                Contestant = guild.Members.FirstOrDefault(m => m.Key == player.UserId).Value.Username,
                Floor = 1,
                Action = "Pending..."
            });
        }

        string output = Formatter.Format(table);
        var embed = new DiscordEmbedBuilder
        {
            Title = "Initial game state", // Optional: set a title,
            
            Description = $"""
                          **Top Floor:** {req.TopFloor}
                          **Current Round:** 1
                          ```
                          {output}
                          ```
                          """, 
            Color = DiscordColor.Blurple // Optional: set the embed color
        };

// Send the embed
        await channel.SendMessageAsync(embed: embed);

    }
}

public class TestSix
{
    public string Contestant { get; set; }
    public int Floor { get; set; }
    public string Action { get; set; }
}