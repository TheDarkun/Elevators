using DSharpPlus.Entities;
using Elevators.Api.Database;
using Elevators.Api.Discord;
using Elevators.Api.Endpoints.Lobby.Requests;
using Elevators.Api.Endpoints.Lobby.Responses;
using FastEndpoints;

namespace Elevators.Api.Endpoints.Lobby;

public class GetLobby : Endpoint<GetLobbyRequest, Elevators.Api.Endpoints.Lobby.Responses.GetLobbyResponse>
{
    public DiscordBot DiscordBot { get; set; }
    public AppDbContext AppDbContext { get; set; }
    
    public override void Configure()
    {
        Get("/lobby/{GuildId}");
    }

    public override async Task HandleAsync(GetLobbyRequest req, CancellationToken ct)
    {
        var game = AppDbContext.Games.FirstOrDefault(g => g.GuildId == req.GuildId);
        var guild = DiscordBot.Client.Guilds.FirstOrDefault(g => g.Key == req.GuildId).Value;
        var lobbyUsers = new List<Elevators.Api.Endpoints.Lobby.Responses.LobbyUser>();
        foreach (var joinedUserId in game.JoinedUsers)
        {
            
            var member = guild.Members.FirstOrDefault(m => m.Key == joinedUserId).Value;
            var user = new Elevators.Api.Endpoints.Lobby.Responses.LobbyUser()
            {
                Name = member.Username,
                Avatar = member.AvatarHash,
                Id = member.Id,
            };
            lobbyUsers.Add(user);
        }

        var channels = new Dictionary<ulong, string>();
        foreach (var channel in guild.Channels.Values)
        {
            if (channel.Type == DiscordChannelType.Text)
            {
                channels.Add(channel.Id, channel.Name);
            } 
        }
        
        await SendOkAsync(new Elevators.Api.Endpoints.Lobby.Responses.GetLobbyResponse()
        {
            Users = lobbyUsers,
            Channels = channels
        }, ct);
    }
}