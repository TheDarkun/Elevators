using Elevators.Api.Database;
using Elevators.Api.Endpoints.User.Requests;
using Elevators.Api.Endpoints.User.Responses;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace Elevators.Api.Endpoints.User;

public class GetUserData : Endpoint<GetUserRequest, Elevators.Api.Endpoints.User.Responses.GetUserResponse>
{
    public AppDbContext AppDbContext { get; set; } = null!;
    public HttpClient HttpClient { get; set; } = null!;
    
    public override void Configure()
    {
        Get("/user");
    }

    public override async Task HandleAsync(GetUserRequest req, CancellationToken ct)
    {
        var user = await AppDbContext.Users.FirstOrDefaultAsync(u => u.SessionId == req.SessionId, ct);
        if (user is null)
        {
            HttpContext.Response.Cookies.Delete("account");
            await SendRedirectAsync("/", true);
            return;
        }
        HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {user.AccessToken}");
        var response = await HttpClient.GetAsync("https://discord.com/api/users/@me");

        if (!response.IsSuccessStatusCode)
        {
            HttpContext.Response.Cookies.Delete("account");
            await SendRedirectAsync("/", true);
            return;
        }

        var content = await response.Content.ReadAsStringAsync();
        var userObject = JObject.Parse(content);

        var userResponse = new Elevators.Api.Endpoints.User.Responses.GetUserResponse()
        {
            Username = userObject["username"]!.ToString(),
            Id = ulong.Parse(userObject["id"]!.ToString()),
            Avatar = userObject["avatar"]!.ToString(),
        };
        await SendOkAsync(userResponse, ct);
    }
}