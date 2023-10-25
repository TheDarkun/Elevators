using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Server.Managers.Dashboard;

public interface IDashboardManager
{
    public Task<IActionResult> GetServerChannels();
    public Task<bool> BotIsJoined(long guildId);
    public Task<IEnumerable<DiscordServer>> GetJoinedServers(string code);   
}