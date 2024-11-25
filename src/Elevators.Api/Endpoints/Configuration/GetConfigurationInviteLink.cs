using Elevators.Api.Endpoints.Configuration.Requests;
using Elevators.Api.Endpoints.Configuration.Responses;
using FastEndpoints;

namespace Elevators.Api.Endpoints.Configuration;

public class GetConfigurationInviteLink : Endpoint<GetConfigurationInviteLinkRequest, GetConfigurationInviteLinkResponse>
{
    public IConfiguration Configuration { get; set; } = null!;   
    
    public override void Configure()
    {
        Get("/configuration/{GuildId}");
    }

    public override async Task HandleAsync(GetConfigurationInviteLinkRequest req, CancellationToken ct)
    {
        var response = new GetConfigurationInviteLinkResponse()
        {
            InviteLink = $"https://discord.com/oauth2/authorize?client_id={Configuration["clientId"]}&permissions=8&integration_type=0&scope=bot&guild_id={req.GuildId}&disable_guild_select=true",
        };
        await SendOkAsync(response, ct);
    }
}