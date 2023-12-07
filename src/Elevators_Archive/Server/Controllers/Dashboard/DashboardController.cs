using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
            {
                Console.WriteLine("No Claims were found");
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            IEnumerable<Claim> claims = identity.Claims;
            var id = claims.FirstOrDefault(c => c.Type == "Id")?.Value;

            if (id is null)
            {
                Console.WriteLine("No Id was found");
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            
            var result = await Manager.GetJoinedGuilds(id);

            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result.Data);
                default:
                    Console.WriteLine(result.Data);
                    return StatusCode((int)result.StatusCode);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{guildId}")]
    [Authorize]
    public async Task<IActionResult> GetGuildChannels(long guildId)
    {
        try
        {
            var result = await Manager.GetGuildChannels(guildId);

            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result.Data);
                default:
                    Console.WriteLine(result.Data);
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
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
            // This is a simple prevention so that when user spams in dashboard all guilds
            await Task.Delay(1000);
            
            var result = await Manager.BotIsJoined(guildId);

            switch (result.StatusCode)
            {
                case HttpStatusCode.InternalServerError:
                    Console.WriteLine(result.Data);
                    return NotFound();
                case HttpStatusCode.NotFound:
                    return NotFound();
                case HttpStatusCode.OK:
                    return Ok();
                default:
                    Console.WriteLine($"There is an error getting different status code in BotIsJoined: {result.StatusCode}");
                    return NotFound();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}