using Elevators.Api.Endpoints.Account.Responses;
using FastEndpoints;

namespace Elevators.Api.Endpoints.Account;

public class GetAccountToken : EndpointWithoutRequest<GetAccountTokenResponse>
{
    public override void Configure()
    {
        Get("/account/token");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        HttpContext.Request.Cookies.TryGetValue("account", out string? token);
        await SendOkAsync(new GetAccountTokenResponse { Token = token }, ct);
    }
}