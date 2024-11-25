namespace Elevators.Api.Endpoints.Lobby.Responses;

public class GetLobbyResponse
{
    public List<LobbyUser> Users { get; set; }
    public Dictionary<ulong, string> Channels { get; set; }
}

public class LobbyUser
{
    public string Name { get; set; }
    public ulong Id { get; set; }
    public string Avatar { get; set; }
}