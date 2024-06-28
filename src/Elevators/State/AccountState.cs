using Newtonsoft.Json;

namespace Elevators.State;

public class AccountState : BaseState
{
    private bool _isLoaded;
    public bool IsLoaded
    {
        get => _isLoaded;
        set
        {
            if (_isLoaded == value)
                return;
            _isLoaded = value;
            OnPropertyChanged();
        }
    }
    
    
    private string _username = "";
    [JsonProperty("username")]
    public string Username
    {
        get => _username;
        set
        {
            if (_username == value)
                return;
            _username = value;
            OnPropertyChanged();
        }
    }

    private string? _displayName;
    [JsonProperty("global_name")]
    public string? DisplayName
    {
        get => _displayName;
        set
        {
            if (_displayName == value)
                return;
            _displayName = value;
            OnPropertyChanged();
        }
    }
    
    private string? _avatar;
    [JsonProperty("avatar")]
    public string? Avatar
    {
        get => _avatar;
        set
        {
            if (_avatar == value)
                return;
            _avatar = value;
            OnPropertyChanged();
        }
    }

    private ulong _id;
    [JsonProperty("id")]
    public ulong Id
    {
        get => _id;
        set
        {
            if (_id == value)
                return;
            _id = value;
            OnPropertyChanged();
        }
    }
}