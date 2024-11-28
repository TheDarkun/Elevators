using Elevators.Api.Database;
using Elevators.Api.Discord;
using Elevators.Api.Endpoints.Rounds.Requests;
using FastEndpoints;

namespace Elevators.Api.Endpoints.Rounds;

public class SendRoundResults : Endpoint<SendRoundResultsRequest>
{
    public ElevatorsManager ElevatorsManager { get; set; } = null!;
    
    public override void Configure()
    {
        Get("round/results/{GuildId}");
    }

    public override async Task HandleAsync(SendRoundResultsRequest req, CancellationToken ct)
    {
        await ElevatorsManager.ShowGameResult(req.GuildId);
    }
}