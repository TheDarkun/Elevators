using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Server.Controllers.Game;

public interface IGameController
{
    public Task<IActionResult> CreateGame(GameCreateDTO game);
}