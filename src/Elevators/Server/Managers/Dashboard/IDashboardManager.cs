using Server.Models;

namespace Server.Managers.Dashboard;

public interface IDashboardManager
{
    public Task<ManagerResult> GetGuildChannels(long guildId);
    public Task<ManagerResult> BotIsJoined(long guildId);
    public Task<ManagerResult> GetJoinedGuilds(string code);   
}