using Elevators.Api.Database;
using Elevators.Api.Endpoints.Lobby.Requests;
using Elevators.Api.Models;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Elevators.Api.Endpoints.Lobby;

public class CreateLobby : Endpoint<CreateLobbyRequest>
{
    public AppDbContext AppDbContext { get; set; } = null!;
    
    public override void Configure()
    {
        Post("lobby");
    }

    public override async Task HandleAsync(CreateLobbyRequest req, CancellationToken ct)
    {
        await AppDbContext.Games.AddAsync(new Game()
        {
            GuildId = req.GuildId,
            JoinedUsers = []
        }, ct);
        var guild = await AppDbContext.SelectedGuilds.FirstOrDefaultAsync(g => g.GuildId == req.GuildId, cancellationToken: ct);
        guild!.Status = Models.GuildStatus.Lobby;
        await AppDbContext.SaveChangesAsync(ct);
        await SendOkAsync(null, ct);
    }
}