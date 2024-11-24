using Elevators.Api.Database;
using Elevators.Api.Endpoints.Guilds.Requests;
using Elevators.Api.Endpoints.Guilds.Responses;
using Elevators.Api.Models;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Elevators.Api.Endpoints.Guilds;

public class GetSelectedGuild : Endpoint<GetSelectedGuildRequest, GetSelectedGuildResponse>
{
    public AppDbContext AppDbContext { get; set; } = null!;
    
    public override void Configure()
    {
        Get("guild/{guildId}");
    }

    public override async Task HandleAsync(GetSelectedGuildRequest req, CancellationToken ct)
    {
        var guild = await AppDbContext.SelectedGuilds.FirstOrDefaultAsync(g => g.GuildId == req.GuildId);
        if (guild is null)
        {
            var selectedGuild = new SelectedGuild()
            {
                Status = GuildStatus.NoBot
            };
            await SendOkAsync(new GetSelectedGuildResponse { SelectedGuild = selectedGuild }, ct);
            return;
        }
        await SendOkAsync(new GetSelectedGuildResponse(), ct);
    }
}