using Microsoft.AspNetCore.Mvc;
using Server.Managers.Game;
using Shared.Models;

namespace Server.Controllers.Game;

[Route("api/[controller]/[action]")]
[ApiController]
public class GameController : Controller, IGameController
{
    private readonly IGameManager manager;

    public GameController(IGameManager manager)
    {
        this.manager = manager;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateGame(GameCreateDTO game)
    {
        try
        {
            await manager.CreateGame(game);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500);
        }
    }
}