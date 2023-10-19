using System.Collections.Immutable;
using Shared.Models;

namespace Server.Managers.Account;

public interface IAccountManager
{
    public Task<string> Authenticate(string code);
    public Task<bool> BotIsJoined(long guildId);
    public Task<IEnumerable<DiscordServer>> GetJoinedServers(string code);
    public Task Logout(string userId);
}