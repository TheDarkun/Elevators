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
            new DiscordSelectComponentOption("Idle", "option2"),
            new DiscordSelectComponentOption("Cut Bureš", "option3"),
            new DiscordSelectComponentOption("Cut Ananas", "option4"),
            new DiscordSelectComponentOption("Cut Úvaha", "option5"),
            new DiscordSelectComponentOption("Cut Úvaha", "option6"),
            new DiscordSelectComponentOption("Cut Úvaha", "option7"),
            new DiscordSelectComponentOption("Cut Úvaha", "option8"),
            new DiscordSelectComponentOption("Cut Úvaha", "option9"),
            new DiscordSelectComponentOption("Cut Úvaha", "option10"),
            new DiscordSelectComponentOption("Cut Úvaha", "option11"),
            new DiscordSelectComponentOption("Cut Úvaha", "option12"),
            new DiscordSelectComponentOption("Cut Úvaha", "option13"),
            new DiscordSelectComponentOption("Cut Úvaha", "option14"),
            new DiscordSelectComponentOption("Cut Úvaha", "option55"),

        };
        var dropdown = new DiscordSelectComponent("dropdown1", "Select your action", dropdownOptions);
        await channel.SendMessageAsync(new DiscordMessageBuilder().WithContent("ahoj").AddComponents(dropdown));
        await SendAsync(new TestEndpointResponse() { Id = req.Id}, 200, ct);
    }
}