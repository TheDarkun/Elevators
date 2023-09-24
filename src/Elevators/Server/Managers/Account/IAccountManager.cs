using System.Collections.Immutable;
using Shared.Models;

namespace Server.Managers.Account;

public interface IAccountManager
{
    public Task<string> Authenticate(string code);
    public Task<IEnumerable<DiscordServer>> GetJoinedServers(string code);
}