// using DSharpPlus;
// using DSharpPlus.Entities;
// using DSharpPlus.SlashCommands;
// using Elevators.Api.Database;
// using FastEndpoints;
// using Microsoft.EntityFrameworkCore;
//
// namespace Elevators.Api.Discord.SlashCommands;
//
// public class LobbyCommands : ApplicationCommandModule
// {
//     public LobbyCommands(AppDbContext appDbContext)
//     {
//         AppDbContext = appDbContext;
//     }
//
//     // public HttpClient HttpClient { get; set; }
//     public AppDbContext AppDbContext { get; set; }
//     
//     [SlashCommand("join", "A slash command made to test the DSharpPlus Slash Commands extension!")]
//     public async Task JoinCommand(InteractionContext ctx)
//     {
//         await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
//
//         await Task.Delay(2000);
//
//         await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("smrdíš prdíš"));
//         
//         /*
//         await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
//         
//         var game = await AppDbContext.Games
//             .FirstOrDefaultAsync(g => g.GuildId == ctx.Guild.Id);
//
//         await ctx.EditResponseAsync( new DiscordWebhookBuilder().WithContent("There is no game on this server"));
//
//         if (game is null)
//         {
//             await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("There is no game on this server"));
//             return;
//         }
//
//         var alreadyJoined = game.JoinedUsers.Contains(ctx.User.Id);
//
//         if (alreadyJoined)
//         {
//             await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("You have already joined the game"));
//             return;
//         }
//         
//         await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Successfully joined"));
//         game.JoinedUsers.Add(ctx.User.Id);
//         await AppDbContext.SaveChangesAsync();
//         */
//     }
//     
//     [SlashCommand("leave", "A slash command made to test the DSharpPlus Slash Commands extension!")]
//     public async Task LeaveCommand(InteractionContext ctx)
//     {
//         await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("pápá"));
//     }
// }