using Elevators.Api.Database;
using Elevators.Api.Endpoints.Lobby.Requests;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Elevators.Api.Endpoints.Lobby;

public class DeleteLobby : Endpoint<DeleteLobbyRequest>
{
    public AppDbContext AppDbContext { get; set; } = null!;
    
    public override void Configure()
    {
        Delete("lobby/{GuildId}");
    }

    public override async Task HandleAsync(DeleteLobbyRequest req, CancellationToken ct)
    {
        await AppDbContext.Games.Where(g => g.GuildId == req.GuildId).ExecuteDeleteAsync(ct);

        var guild = await AppDbContext.SelectedGuilds.FirstOrDefaultAsync(g => g.GuildId == req.GuildId, cancellationToken: ct);
        guild!.Status = Models.GuildStatus.NoGame;
        await AppDbContext.SaveChangesAsync(ct);
        await SendOkAsync(null, ct);
    }
}