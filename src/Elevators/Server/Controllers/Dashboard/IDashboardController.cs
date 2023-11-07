using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers.Dashboard;

public interface IDashboardController
{
    public Task<IActionResult> BotIsJoined(long guildId);
    public Task<IActionResult> GetJoinedGuilds();
    public Task<IActionResult> GetGuildChannels(long guildId);
}