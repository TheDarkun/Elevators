using Elevators.Api.Database;
using Elevators.Api.Discord;
using Elevators.Api.Endpoints.Rounds.Requests;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Elevators.Api.Endpoints.Rounds;

public class StartNewRound : Endpoint<StartNewRoundRequest>
{
    public AppDbContext AppDbContext { get; set; } = null!;
    public ElevatorsManager ElevatorsManager { get; set; } = null!;
    
    public override void Configure()
    {
        Get("round/{GuildId}/next");
    }

    public override async Task HandleAsync(StartNewRoundRequest req, CancellationToken ct)
    {
        var round = await AppDbContext.Rounds.Include(round => round.Players).FirstOrDefaultAsync(r => r.GuildId == req.GuildId);
        round.CurrentRound++;
        foreach (var player in round.Players)
        {
            player.PlayerAction = PlayerAction.NoAction;
            player.CutPlayerId = null;
        }
        await AppDbContext.SaveChangesAsync(ct);
        await ElevatorsManager.ShowGameStatus(req.GuildId);
    }
}