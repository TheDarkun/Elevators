using Microsoft.AspNetCore.Mvc;

namespace Elevators.Controllers;

public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Hello()
    {
        return Ok("Hi");
    }
}

