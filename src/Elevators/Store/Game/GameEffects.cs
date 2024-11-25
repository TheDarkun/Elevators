using Elevators.Store.Game.Actions;
using Elevators.Store.SelectedGuild.Actions;
using Fluxor;

namespace Elevators.Store.Game;

public class GameEffects(ApiClient apiClient)
{
    [EffectMethod]
    public async Task HandleStartGameAction(StartGameAction action, IDispatcher dispatcher)
    {
        try
        {
            await apiClient.CreateGameAsync(new CreateGameRequest()
            {
                GuildId = action.GuildId,
                TopFloor = action.FloorCount,
                GameRoomId = action.ChannelId
            });
        }
        catch (Exception e)
        {
            var selectedGuild = await apiClient.GetSelectedGuildAsync(action.GuildId);
            dispatcher.Dispatch(new FetchSelectedGuildResultAction(selectedGuild));
        }
    }
    
    [EffectMethod]
    public async Task HandleDeleteGameAction(DeleteGameAction action, IDispatcher dispatcher)
    {
        try
        {
            await apiClient.DeleteGameAsync(action.GuildId);
        }
        catch (Exception e)
        {
            var selectedGuild = await apiClient.GetSelectedGuildAsync(action.GuildId);
            dispatcher.Dispatch(new FetchSelectedGuildResultAction(selectedGuild));
        }
    }

    [EffectMethod]
    public async Task HandleFetchCurrentRoundAction(FetchCurrentRoundAction action, IDispatcher dispatcher)
    {
        var round = await apiClient.GetCurrentRoundAsync(action.GuildId);
        dispatcher.Dispatch(new FetchCurrentRoundResultAction(round));
    }
}