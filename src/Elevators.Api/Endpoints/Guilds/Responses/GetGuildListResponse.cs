using Elevators.Api.Models;

namespace Elevators.Api.Endpoints.Guilds.Responses;

public class GetGuildListResponse
{
    public List<Models.Guild> Guilds { get; set; } = new ();
}