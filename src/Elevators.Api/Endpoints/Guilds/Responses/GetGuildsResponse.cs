using Elevators.Api.Models;

namespace Elevators.Api.Endpoints.Guilds.Responses;

public class GetGuildsResponse
{
    public List<Models.Guild> Guilds { get; set; } = new ();
}