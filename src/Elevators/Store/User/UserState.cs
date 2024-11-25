using Fluxor;

namespace Elevators.Store.User;

[FeatureState]
public class UserState
{
    public string Username { get; }
    public string Avatar { get; }
    public ulong Id { get; }
    public bool IsLoading { get; } = true;
    
    private UserState() {} // Required for creating initial state

    public UserState(string username, string avatar, ulong id, bool isLoading = false)
    {
        Username = username;
        Avatar = avatar;
        Id = id;
        IsLoading = isLoading;
    }
}