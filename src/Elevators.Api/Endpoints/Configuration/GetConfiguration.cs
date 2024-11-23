using Elevators.Api.Endpoints.Configuration.Responses;
using FastEndpoints;

namespace Elevators.Api.Endpoints.Configuration;

public class GetConfiguration : EndpointWithoutRequest<GetConfigurationResponse>
{
    public IConfiguration Configuration { get; set; } = null!;   
    
    public override void Configure()
    {
        Get("/configuration");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = new GetConfigurationResponse()
        {
            Oauth2Uri = Configuration["uri:oauth2"] ?? "",
            BotUri = Configuration["uri:bot"] ?? "",
        };
        await SendOkAsync(response, ct);
    }
}