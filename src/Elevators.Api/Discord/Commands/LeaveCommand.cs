using DSharpPlus.Commands;
using Elevators.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace Elevators.Api.Discord.Commands;

public class LeaveCommand
{
    [Command("leave")]
    public static async ValueTask ExecuteAsync(CommandContext context)
    {
        await context.RespondAsync($"Loading...");
        var appDbContext = context.ServiceProvider.GetService<AppDbContext>();

        var game = await appDbContext.Games
            .FirstOrDefaultAsync(g => g.GuildId == context.Guild.Id);

        if (game is null)
        {
            await context.EditResponseAsync("There is no game on this server");
            return;
        }

        // TODO: leave when game began
        
        var joined = game.JoinedUsers.Contains(context.User.Id);

        if (joined)
        {
            await context.EditResponseAsync("Successfully left the game");
            game.JoinedUsers.Remove(context.User.Id);
            await appDbContext.SaveChangesAsync();
            return;
        }

        await context.EditResponseAsync("You are not in this game");
    }
}