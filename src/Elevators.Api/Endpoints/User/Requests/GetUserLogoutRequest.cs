using FastEndpoints;

namespace Elevators.Api.Endpoints.User.Requests;

public class GetUserLogoutRequest
{
    [FromClaim]
    public Guid SessionId { get; set; }
}