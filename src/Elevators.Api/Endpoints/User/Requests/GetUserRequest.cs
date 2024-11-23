using FastEndpoints;

namespace Elevators.Api.Endpoints.User.Requests;

public class GetUserRequest
{
    [FromClaim]
    public Guid SessionId { get; set; }
}