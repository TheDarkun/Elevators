using Elevators.Api.Database;
using Elevators.Api.Discord;
using Elevators.Api.Endpoints.Rounds.Requests;
using Elevators.Api.Endpoints.Rounds.Responses;
using Elevators.Api.Models;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Elevators.Api.Endpoints.Rounds;

public class GetCurrentRound : Endpoint<GetCurrentRoundRequest, Elevators.Api.Endpoints.Rounds.Responses.GetCurrentRoundResponse>
{
    public AppDbContext AppDbContext { get; set; } = null!;
    public DiscordBot DiscordBot { get; set; } = null!;
    
    public override void Configure()
    {
        Get("round/{GuildId}");
    }

    public override async Task HandleAsync(GetCurrentRoundRequest req, CancellationToken ct)
    {
        var round = await AppDbContext.Rounds.Include(round => round.Players).FirstOrDefaultAsync(r => r.GuildId == req.GuildId, ct);
        bool gameFinished = true;
        foreach (var player in round.Players.Where(player => player.PlayerAction == PlayerAction.NoAction))
        {
            gameFinished = false;
        }

        var guild = DiscordBot.Client.Guilds.FirstOrDefault(g => g.Key == req.GuildId).Value;
        var game = await AppDbContext.Games.FirstOrDefaultAsync(g => g.GuildId == req.GuildId, ct);
        List<Elevators.Api.Endpoints.Rounds.Responses.GetCurrentRoundPlayerResponse> players = new();
        Elevators.Api.Endpoints.Rounds.Responses.GetCurrentRoundResponse response;
        if (gameFinished)
        {
            foreach (var player in round.Players)
            {
                players.Add(new Elevators.Api.Endpoints.Rounds.Responses.GetCurrentRoundPlayerResponse()
                {
                    Name = guild.Members.FirstOrDefault(m => m.Key == player.UserId).Value.Username,
                    Floor = player.Floor,
                    Avatar = guild.Members.FirstOrDefault(m => m.Key == player.UserId).Value.AvatarHash,
                    PlayerAction = player.PlayerAction,
                    UserId = player.UserId,
                    IsAlive = player.IsAlive,
                    CutPlayerId = player.CutPlayerId,
                    Submitted = true
                });
            }
            response = new Elevators.Api.Endpoints.Rounds.Responses.GetCurrentRoundResponse()
            {
                Players = players,
                IsFinished = gameFinished,
                CurrentRound = round.CurrentRound,
                TopFloor = game.TopFloor
            };
            await SendOkAsync(response, ct);
            return;
        }
        
        foreach (var player in round.Players)
        {
            players.Add(new Elevators.Api.Endpoints.Rounds.Responses.GetCurrentRoundPlayerResponse()
            {
                Name = guild.Members.FirstOrDefault(m => m.Key == player.UserId).Value.Username,
                Floor = player.Floor,
                Avatar = guild.Members.FirstOrDefault(m => m.Key == player.UserId).Value.AvatarHash,
                PlayerAction = PlayerAction.NoAction,
                UserId = player.UserId,
                IsAlive = player.IsAlive,
                CutPlayerId = player.CutPlayerId,
                Submitted = player.PlayerAction != PlayerAction.NoAction
            });
        }
        response = new Elevators.Api.Endpoints.Rounds.Responses.GetCurrentRoundResponse()
        {
            Players = players,
            IsFinished = gameFinished,
            CurrentRound = round.CurrentRound,
            TopFloor = game.TopFloor,
        };
        await SendOkAsync(response, ct);

    }
}