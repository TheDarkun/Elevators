using DSharpPlus.Commands;
using DSharpPlus.Entities;
using Elevators.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace Elevators.Api.Discord.Commands;

public class JoinCommand
{
    [Command("join")]
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

        var alreadyJoined = game.JoinedUsers.Contains(context.User.Id);

        if (alreadyJoined)
        {
            await context.EditResponseAsync("You have already joined the game");
            return;
        }

        await context.EditResponseAsync("Successfully joined");
        game.JoinedUsers.Add(context.User.Id);
        await appDbContext.SaveChangesAsync();
    }
}