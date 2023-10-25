using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Server.Managers.Dashboard;

public interface IDashboardManager
{
    public Task<IEnumerable<DiscordChannel>> GetGuildChannels(long guildId);
    public Task<bool> BotIsJoined(long guildId);
    public Task<IEnumerable<DiscordServer>> GetJoinedGuilds(string code);   
}