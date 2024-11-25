using Elevators.Store.Game.Actions;
using Fluxor;

namespace Elevators.Store.Game;

public class GameReducers
{
    [ReducerMethod]
    public static GameState ReduceFetchCurrentRoundAction(GameState gameState, FetchCurrentRoundAction action)
        => new GameState(true, gameState.IsFinished, gameState.CurrentRound, gameState.TopFloor, gameState.Players);

    [ReducerMethod]
    public static GameState ReduceFetchCurrentRoundResultAction(GameState gameState, FetchCurrentRoundResultAction action)
        => new GameState(false, action.Response.IsFinished, action.Response.CurrentRound, action.Response.TopFloor, action.Response.Players.Select(p => new Player
        {
            Name = p.Name,
            UserId = p.UserId,
            Avatar = p.Avatar,
            IsAlive = p.IsAlive,
            PlayerAction = p.PlayerAction,
            Floor = p.Floor,
            CutPlayerId = p.CutPlayerId,
            Submitted = p.Submitted
        }).ToList());
}