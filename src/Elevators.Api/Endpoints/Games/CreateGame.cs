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
    public ElevatorsManager ElevatorsManager { get; set; } = null!;
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

        await ElevatorsManager.ShowGameStatus(req.GuildId);
        // await ElevatorsManager.ShowGameActionSelect(req.GuildId);
    }
}