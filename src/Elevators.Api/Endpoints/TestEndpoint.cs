using DSharpPlus;
using DSharpPlus.Entities;
using Elevators.Api.Discord;
using FastEndpoints;

namespace Elevators.Api.Endpoints;

public class TestEndpointRequest
{
    public int Id { get; set; }
}

public class TestEndpointResponse
{
    public int Id { get; set; }
}

public class TestEndpoint : Endpoint<TestEndpointRequest, TestEndpointResponse>
{
    public DiscordBot DiscordBot { get; set; }
    
    public override void Configure()
    {
        Get("/test/{Id}");
    }

    public override async Task HandleAsync(TestEndpointRequest req, CancellationToken ct)
    {
        var guild = DiscordBot.Client.Guilds.Single(G => G.Key == 1310185556193316936).Value;
        var channel = guild.Channels.Single(ch => ch.Key == 1310279797657567252).Value;
        var button1 = new DiscordButtonComponent(DiscordButtonStyle.Secondary, "up", "Up");
        var button2 = new DiscordButtonComponent(DiscordButtonStyle.Primary, "idle", "Idle");
        var dropdownOptions = new List<DiscordSelectComponentOption>
        {
            new DiscordSelectComponentOption("Up", "option1"),
        };
        var dropdown = new DiscordSelectComponent("dropdown1", "Select your action", dropdownOptions);
        await channel.SendMessageAsync(new DiscordMessageBuilder().WithContent("ahoj").AddComponents(dropdown));
        await SendAsync(new TestEndpointResponse() { Id = req.Id}, 200, ct);
    }
}