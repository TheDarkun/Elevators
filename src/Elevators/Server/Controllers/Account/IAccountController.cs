using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers.Account;

public interface IAccountController
{
    public IActionResult Authorize();
    public Task<IActionResult> Authenticate();
    public Task<IActionResult> BotIsJoined(long guildId);
    public Task<IActionResult> GetJoinedServers();
    public Task<IActionResult> Logout();
}