using Shared.Models;

namespace Server.Managers.Game;

public interface IGameManager
{
    public Task CreateGame(GameCreateDTO game);
}