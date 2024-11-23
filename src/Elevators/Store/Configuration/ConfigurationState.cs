using Fluxor;

namespace Elevators.Store.Configuration;

[FeatureState]
public class ConfigurationState
{
    public string Oauth2Uri { get; }
    public string BotUri { get; }
    public bool IsLoading { get; }
    
    private ConfigurationState() {} // Required for creating initial state

    public ConfigurationState(string oauth2Uri, string botUri, bool isLoading)
    {
        Oauth2Uri = oauth2Uri;
        BotUri = botUri;
        IsLoading = isLoading;
    }

}