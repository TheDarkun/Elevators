namespace Elevators.Api.Endpoints.User.Responses;

public class GetUserResponse
{
    public required string Username { get; set; }
    public required string Avatar { get; set; }
    public required ulong Id { get; set; }
}