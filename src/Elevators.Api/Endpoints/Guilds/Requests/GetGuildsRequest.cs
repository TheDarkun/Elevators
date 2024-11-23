using FastEndpoints;

namespace Elevators.Api.Endpoints.Guilds.Requests;

public class GetGuildsRequest
{
    [FromClaim]
    public Guid SessionId { get; set; }
}