using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers.Account;

public interface IAccountController
{
    public IActionResult Authorize();
    public Task<IActionResult> Authenticate();
    public Task<IActionResult> Logout();
    public Task<IActionResult> GetJoinedServers();
}