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

    public DashboardController(IDashboardManager manager) => Manager = manager;
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetJoinedGuilds()
    {
        try
        {
            if (HttpContext.User.Identity is not ClaimsIdentity identity)
                return StatusCode(StatusCodes.Status401Unauthorized, "No Claims were found");

            IEnumerable<Claim> claims = identity.Claims;
            var id = claims.FirstOrDefault(c => c.Type == "Id")?.Value;

            if (id is null)
                return StatusCode(StatusCodes.Status401Unauthorized, "No Id was found");
            
            var servers = await Manager.GetJoinedGuilds(id);
            return Ok(servers);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500);
        }
    }

    [HttpGet("{guildId}")]
    [Authorize]
    public async Task<IActionResult> GetGuildChannels(long guildId)
    {
        try
        {
            var result = await Manager.GetGuildChannels(guildId);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500);
        }
    }
    
    [HttpGet("{guildId}")]
    [Authorize]
    public async Task<IActionResult> BotIsJoined(long guildId)
    {
        try
        {
            // This is a simple prevencion so that when user spams in dashboard all servers, discord API does not shout errors for spam
            await Task.Delay(1000);
            
            var result = await Manager.BotIsJoined(guildId);

            if (result)
                return Ok();

            return NotFound();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500);
        }
    }
}