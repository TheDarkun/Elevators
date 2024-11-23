using Elevators.Api.Database;
using Elevators.Api.Endpoints.User.Requests;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Elevators.Api.Endpoints.User;

public class GetUserLogout : Endpoint<GetUserLogoutRequest>
{
    public AppDbContext AppDbContext { get; set; } = null!;
    
    public override void Configure()
    {
        Get("user/logout");
    }

    public override async Task HandleAsync(GetUserLogoutRequest req, CancellationToken ct)
    {
        await AppDbContext.Users.Where(u => u.SessionId == req.SessionId).ExecuteDeleteAsync(ct);
        HttpContext.Response.Cookies.Delete("account");
        await SendNoContentAsync(ct);
    }
}