using Elevators.Api.Models;

namespace Elevators.Api.Endpoints.Guilds.Responses;

public class GetSelectedGuildResponse
{
    public Models.GuildStatus GuildStatus { get; set; }
    public int MemberCount { get; set; }
}