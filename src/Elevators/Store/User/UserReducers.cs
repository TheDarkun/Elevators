using Elevators.Store.User.Actions;
using Fluxor;

namespace Elevators.Store.User;

public class UserReducers
{
    [ReducerMethod]
    public static UserState ReduceFetchUserAction(UserState userState, FetchUserAction action)
        => new UserState("", "", 0, true);
    
    [ReducerMethod]
    public static UserState ReduceFetchUserResultAction(UserState userState, FetchUserActionResult action)
        => new UserState(action.Response.Username, action.Response.Avatar, action.Response.Id, false);
}