using Elevators.Store.Configuration.Actions;
using Fluxor;

namespace Elevators.Store.Configuration;

public class ConfigurationEffects
{
    private readonly ApiClient _apiClient;

    public ConfigurationEffects(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [EffectMethod]
    public async Task HandleFetchConfigurationAction(FetchConfigurationAction action, IDispatcher dispatcher)
    {
        var configuration = await _apiClient.GetConfigurationAsync();
        dispatcher.Dispatch(new FetchConfigurationResultAction(configuration));
    }
}