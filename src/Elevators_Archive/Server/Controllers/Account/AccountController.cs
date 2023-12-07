using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Managers.Account;

namespace Server.Controllers.Account;

[Route("api/[controller]/[action]")]
[ApiController]
public class AccountController : Controller, IAccountController
{
    private IAccountManager Manager { get; }
    public AccountController(IAccountManager manager)
    {
        Manager = manager;
    }
    
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Authorize() 
        => Redirect("https://discord.com/api/oauth2/authorize?client_id=1135226184217677926&redirect_uri=http%3A%2F%2Flocalhost%3A5000%2Fapi%2FAccount%2FAuthenticate&response_type=code&scope=identify%20guilds%20guilds.members.read");

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Authenticate()
    {
        string? code = HttpContext.Request.Query["code"];

        if (code is null)
            return Forbid();
        
        var result = await Manager.Authenticate(code);

        switch (result.StatusCode)
        {
            case HttpStatusCode.OK:
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(7), // Cookie will expire in 7 days
                    HttpOnly = false, // Cookie is only accessible through HTTP (not JavaScript)
                    Secure = true, // Cookie will only be sent over HTTPS
                    SameSite = SameSiteMode.Strict, // Protect against cross-site request forgery (CSRF) attacks
                };
            
                // Set the cookie value
                Response.Cookies.Append("account", result.Data!.ToString()!, cookieOptions);
            
                return Redirect("/Dashboard");
            default:
                Console.WriteLine(result.Data);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred");
        }
        
    }

    // [HttpGet]
    // [Authorize]
    // public async Task<IActionResult> GetJoinedServers()
    // {
    //     try
    //     {
    //         var identity = HttpContext.User.Identity as ClaimsIdentity;
    //
    //         if (identity == null)
    //             return StatusCode(StatusCodes.Status401Unauthorized, "No Claims were found");
    //
    //         IEnumerable<Claim> claims = identity.Claims;
    //         var id = claims.FirstOrDefault(c => c.Type == "Id")?.Value;
    //
    //         if (id is null)
    //             return StatusCode(StatusCodes.Status401Unauthorized, "No Id was found");
    //         
    //         var servers = await Manager.GetJoinedServers(id);
    //         return Ok(servers);
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         return StatusCode(StatusCodes.Status500InternalServerError);
    //     }
    // }

    // [HttpGet("{guildId}")]
    // [Authorize]
    // public async Task<IActionResult> BotIsJoined(long guildId)
    // {
    //     try
    //     {
    //         await Task.Delay(1000);
    //         
    //         var result = await Manager.BotIsJoined(guildId);
    //
    //         if (result)
    //             return Ok();
    //
    //         return NotFound();
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         return StatusCode(StatusCodes.Status500InternalServerError);
    //     }
    // }
    
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Logout()
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
            
            var result = await Manager.Logout(id);

            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    return NoContent();
                default:
                    Console.WriteLine(result.StatusCode);
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}