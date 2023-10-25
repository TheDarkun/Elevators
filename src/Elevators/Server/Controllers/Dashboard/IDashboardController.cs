using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers.Dashboard;

public interface IDashboardController
{
    public Task<IActionResult> BotIsJoined(long guildId);
    public Task<IActionResult> GetJoinedServers();
    public Task<IActionResult> GetServerChannels();
}