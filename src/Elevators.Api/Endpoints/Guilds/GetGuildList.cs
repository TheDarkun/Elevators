using Elevators.Api.Database;
using Elevators.Api.Endpoints.Guilds.Requests;
using Elevators.Api.Endpoints.Guilds.Responses;
using Elevators.Api.Models;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace Elevators.Api.Endpoints.Guilds;

public class GetGuildList : Endpoint<GetGuildListRequest, GetGuildsResponse>
{
    public AppDbContext AppDbContext { get; set; } = null!;
    public HttpClient HttpClient { get; set; } = null!;
    
    public override void Configure()
    {
        Get("/guilds");
    }

    public override async Task HandleAsync(GetGuildListRequest req, CancellationToken ct)
    {
        var user = await AppDbContext.Users.FirstOrDefaultAsync(u => u.SessionId == req.SessionId);
        if (user is null)
        {
            HttpContext.Response.Cookies.Delete("account");
            await SendRedirectAsync("/", true);
            return;
        }
        HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {user.AccessToken}");
        var response = await HttpClient.GetAsync("https://discord.com/api/users/@me/guilds");

        if (!response.IsSuccessStatusCode)
        {
            HttpContext.Response.Cookies.Delete("account");
            await SendRedirectAsync("/", true);
            return;
        }
        // 2147483647 
        var content = await response.Content.ReadAsStringAsync();
        var guildsObject = JArray.Parse(content);

        var guilds = new List<Guild>();
        foreach (var guildObject in guildsObject)
        {
            if (guildObject["permissions"]?.ToString() != "2147483647") continue;
            var guild = new Guild()
            {
                Name = guildObject["name"]!.ToString(),
                Id = long.Parse(guildObject["id"]!.ToString()),
                Icon = guildObject["icon"]!.ToString(),
            };
            guilds.Add(guild);
        }

        await SendOkAsync(new GetGuildsResponse() { Guilds = guilds }, ct);
    }
}