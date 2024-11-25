using Elevators.Api.Database;
using Elevators.Api.Discord;
using Elevators.Api.Endpoints.Guilds.Requests;
using Elevators.Api.Endpoints.Guilds.Responses;
using Elevators.Api.Models;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Elevators.Api.Endpoints.Guilds;

public class GetSelectedGuild : Endpoint<Elevators.Api.Endpoints.Guilds.Requests.GetSelectedGuildRequest, Elevators.Api.Endpoints.Guilds.Responses.GetSelectedGuildResponse>
{
    public AppDbContext AppDbContext { get; set; } = null!;
    public DiscordBot DiscordBot { get; set; } = null!;
    public override void Configure()
    {
        Get("guild/{guildId}");
    }

    public override async Task HandleAsync(Elevators.Api.Endpoints.Guilds.Requests.GetSelectedGuildRequest req, CancellationToken ct)
    {
        // var guild = await AppDbContext.SelectedGuilds.FirstOrDefaultAsync(g => g.GuildId == req.GuildId);
        /*
        if (guild is null)
        {
            var selectedGuild = new SelectedGuild()
            {
                Status = GuildStatus.NoBot
            };
            await SendOkAsync(new GetSelectedGuildResponse { SelectedGuild = selectedGuild }, ct);
            return;
        }*/

        if (DiscordBot.Client.Guilds.TryGetValue(req.GuildId, out var guild))
        {
            var selectedGuild = await AppDbContext.SelectedGuilds.FirstOrDefaultAsync(g => g.GuildId == guild.Id);

            if (selectedGuild is null)
            {
                AppDbContext.SelectedGuilds.Add(new Models.SelectedGuild()
                {
                    GuildId = req.GuildId,
                    Status = Models.GuildStatus.NoGame,
                });
                await AppDbContext.SaveChangesAsync();
            }
            else
            {
                await SendOkAsync(new Elevators.Api.Endpoints.Guilds.Responses.GetSelectedGuildResponse
                {
                    GuildStatus = selectedGuild.Status,
                    MemberCount = guild.MemberCount
                }, ct);
                return;
            }
            await SendOkAsync(new Elevators.Api.Endpoints.Guilds.Responses.GetSelectedGuildResponse
            {
                GuildStatus = Models.GuildStatus.NoGame,
                MemberCount = guild.MemberCount
            }, ct);
            return;
        }
        else
        {
            await SendOkAsync(new Elevators.Api.Endpoints.Guilds.Responses.GetSelectedGuildResponse
            {
                GuildStatus = Models.GuildStatus.NoBot,
            }, ct);
            return;
        }
    }
}