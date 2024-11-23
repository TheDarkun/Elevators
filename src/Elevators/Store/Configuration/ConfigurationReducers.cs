using Elevators.Store.Configuration.Actions;
using Fluxor;

namespace Elevators.Store.Configuration;

public static class ConfigurationReducers
{
    [ReducerMethod]
    public static ConfigurationState ReduceFetchConfigurationAction(ConfigurationState state, FetchConfigurationAction action) =>
        new ConfigurationState("", "", true);

    [ReducerMethod]
    public static ConfigurationState ReduceFetchConfigurationActionResultAction(ConfigurationState state,
        FetchConfigurationResultAction action) => new ConfigurationState(action.Response.Oauth2Uri, action.Response.BotUri, false);
}