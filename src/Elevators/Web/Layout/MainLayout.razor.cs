using Elevators.State;
using Microsoft.AspNetCore.Components;
// using Microsoft.AspNetCore.Components.Authorization;

namespace Elevators.Web.Layout;

public partial class MainLayout
{
    // [Inject]
    // public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    //
    // [Inject] 
    // public IHttpClientFactory HttpClientFactory { get; set; } = null!;
    
    [Inject]
    public ILogger<MainLayout> Logger { get; set; } = null!;
    
    // protected override async Task OnInitializedAsync()
    // {
    //     var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
    //     var userId = state.User.Claims.FirstOrDefault();
    //     
    //     if (userId is null)
    //         return;
    //     
    //     var httpClient = HttpClientFactory.CreateClient("discord");
    //
    //     var path = $"users/{userId.Value}";
    //     Logger.LogInformation("Getting user info using {Path}", path);
    //     var response = await httpClient.GetAsync(path);
    //     
    //     if (response.IsSuccessStatusCode)
    //     {
    //         var content = await response.Content.ReadAsStringAsync();
    //         var account = JsonConvert.DeserializeObject<AccountState>(content)!;
    //
    //         Logger.LogInformation("Successfully retrieved user info \nUsername is {Username} \nDisplayName is{DisplayName} \nAvatar is {Avatar}",
    //             account.Username, account.DisplayName, account.Avatar);
    //         
    //         // The accountState has to be assigned like this and not overriden with the account variable because it would remove the StateChange behaviour that those properties have
    //         AccountState.Id = account.Id;
    //         AccountState.Username = account.Username;
    //         AccountState.DisplayName = account.DisplayName;
    //         AccountState.Avatar = account.Avatar;
    //         AccountState.IsLoaded = true;
    //
    //         /* For example this wont do anything
    //         AccountState = new AccountState();
    //         AccountState.IsLoaded = true;
    //         */
    //     }
    //     else
    //     {
    //         Logger.LogError("Failed to retrieve user info. Status code: {StatusCode}", response.StatusCode);
    //     }
    // }
}