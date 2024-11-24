using FastEndpoints;

namespace Elevators.Api.Endpoints.Guilds.Requests;

public class GetGuildListRequest
{
    [FromClaim]
    public Guid SessionId { get; set; }
}