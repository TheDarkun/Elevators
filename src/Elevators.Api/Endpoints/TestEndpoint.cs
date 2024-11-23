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
    public override void Configure()
    {
        Get("/test/{Id}");
    }

    public override async Task HandleAsync(TestEndpointRequest req, CancellationToken ct)
    {
        await SendAsync(new TestEndpointResponse() { Id = req.Id}, 200, ct);
    }
}