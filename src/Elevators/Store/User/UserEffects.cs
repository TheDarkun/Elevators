using Elevators.Store.User.Actions;
using Fluxor;

namespace Elevators.Store.User;

public class UserEffects(ApiClient apiClient)
{
    
    [EffectMethod]
    public async Task HandleFetchUserAction(FetchUserAction action, IDispatcher dispatcher)
    {
        var user = await apiClient.GetUserDataAsync();
        dispatcher.Dispatch(new FetchUserActionResult(user));
    }
}