using Elevators.Api.Database;
using Elevators.Api.Endpoints.Games.Requests;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Elevators.Api.Endpoints.Games;

public class DeleteGame : Endpoint<DeleteGameRequest>
{
    public AppDbContext AppDbContext { get; set; } = null!;
    
    public override void Configure()
    {
        Delete("/games/{GuildId}");
    }

    public override async Task HandleAsync(DeleteGameRequest req, CancellationToken ct)
    {
        await AppDbContext.Rounds.Where(r => r.GuildId == req.GuildId).ExecuteDeleteAsync(ct);
        await AppDbContext.Games.Where(g => g.GuildId == req.GuildId).ExecuteDeleteAsync(ct);
        var selectedGuild = AppDbContext.SelectedGuilds.Single(g => g.GuildId == req.GuildId);
        selectedGuild.Status = Models.GuildStatus.NoGame;
        await AppDbContext.SaveChangesAsync(ct);
        await SendNoContentAsync(ct);
    }
}