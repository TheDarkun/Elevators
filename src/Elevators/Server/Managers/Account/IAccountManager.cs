namespace Server.Managers.Account;

public interface IAccountManager
{
    public Task<string> Authenticate(string code);
}