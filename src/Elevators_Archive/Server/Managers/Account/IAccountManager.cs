using Server.Models;

namespace Server.Managers.Account;

public interface IAccountManager
{
    public Task<ManagerResult> Authenticate(string code);
    public Task<ManagerResult>Logout(string userId);
}