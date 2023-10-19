namespace Web.Connectors;

public interface ILogoutConnector
{
    public Task Logout();
}

public class LogoutConnector : ILogoutConnector
{
    private HttpClient Client { get; }

    public LogoutConnector(HttpClient client)
    {
        Client = client;
    }
    
    public async Task Logout()
    {
        await Client.DeleteAsync("api/Account/Logout");
    }
}