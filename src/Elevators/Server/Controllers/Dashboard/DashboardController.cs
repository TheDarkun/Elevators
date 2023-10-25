using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Managers.Dashboard;

namespace Server.Controllers.Dashboard;

[Route("api/[controller]/[action]")]
[ApiController]
public class DashboardController : Controller, IDashboardController
{
    private IDashboardManager Manager { get; }

    public DashboardController(IDashboardManager manager)
    {
        Manager = manager;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetJoinedServers()
    {
        try
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return StatusCode(StatusCodes.Status401Unauthorized, "No Claims were found");

            IEnumerable<Claim> claims = identity.Claims;
            var id = claims.FirstOrDefault(c => c.Type == "Id")?.Value;

            if (id is null)
                return StatusCode(StatusCodes.Status401Unauthorized, "No Id was found");
            
            var servers = await Manager.GetJoinedServers(id);
            return Ok(servers);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{guildId}")]
    [Authorize]
    public async Task<IActionResult> GetServerChannels(long guildId)
    {
        try
        {
            var result = Manager.
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{guildId}")]
    [Authorize]
    public async Task<IActionResult> BotIsJoined(long guildId)
    {
        try
        {
            await Task.Delay(1000);
            
            var result = await Manager.BotIsJoined(guildId);

            if (result)
                return Ok();

            return NotFound();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}