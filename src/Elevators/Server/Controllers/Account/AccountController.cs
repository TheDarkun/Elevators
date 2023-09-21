using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Managers.Account;

namespace Server.Controllers.Account;

[Route("api/[controller]/[action]")]
[ApiController]
public class AccountController : Controller, IAccountController
{
    private IAccountManager manager { get; }
    public AccountController(IAccountManager manager)
    {
        this.manager = manager;
    }
    
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Authorize()
    {
        return Redirect("https://discord.com/api/oauth2/authorize?client_id=1135226184217677926&redirect_uri=http%3A%2F%2Flocalhost%3A5000%2Fapi%2FAccount%2FAuthenticate&response_type=code&scope=identify");
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Authenticate()
    {
        try
        {
            string? code = HttpContext.Request.Query["code"];

            if (code is null)
                throw new Exception();
            
            var jwtToken = await manager.Authenticate(code);
            
            var cookieOptions = new CookieOptions
            {
                // Set cookie properties as needed, such as expiration time, domain, etc.
                // For example:
                Expires = DateTimeOffset.Now.AddDays(1), // Cookie will expire in 1 day
                HttpOnly = false, // Cookie is only accessible through HTTP (not JavaScript)
                Secure = true, // Cookie will only be sent over HTTPS
                SameSite = SameSiteMode.Strict, // Protect against cross-site request forgery (CSRF) attacks
                // You can add other properties as needed
            };
            
            // Set the cookie value
            Response.Cookies.Append("account", jwtToken, cookieOptions);
            
            return Redirect("/Dashboard");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        try
        {
            return Ok("yo wassup");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}