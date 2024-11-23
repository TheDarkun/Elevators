using Fluxor;

namespace Elevators.Store.User;

[FeatureState]
public class UserState
{
    public string Username { get; }
    public string Avatar { get; }
    public long Id { get; }
    public bool IsLoading { get; } = true;
    
    private UserState() {} // Required for creating initial state

    public UserState(string username, string avatar, long id, bool isLoading = false)
    {
        Username = username;
        Avatar = avatar;
        Id = id;
        IsLoading = isLoading;
    }
}