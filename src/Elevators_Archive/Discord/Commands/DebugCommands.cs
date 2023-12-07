using System.Diagnostics;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Discord.Commands;

public class DebugCommands : ApplicationCommandModule
{
     [SlashCommand("Ping", "Check if the bot can respond to simple commands")]
     public async Task Ping(InteractionContext ctx)
     {
          var before = DateTime.UtcNow; // Capture the time before sending the message

          // Send a simple message to measure just the bot's processing time
          await ctx.CreateResponseAsync("Pong!");

          var after = DateTime.UtcNow; // Capture the time after receiving the response

          var responseTime = after - before; // Calculate the response time

          // Create a DiscordWebhookBuilder to edit the response
          var webhookBuilder = new DiscordWebhookBuilder()
               .WithContent($"Pong! Response Time: {responseTime.TotalMilliseconds}ms");

          await ctx.EditResponseAsync(webhookBuilder);
     }

     [SlashCommand("Backend", "Check if a bot is able to send requests to the backend")]
     public async Task Backend(InteractionContext ctx)
     {
          try
          {
               using (var client = new HttpClient())
               {
                    var response = await client.GetAsync("http://localhost:5000/");
                    if (string.IsNullOrWhiteSpace(response.StatusCode.ToString()))
                    {
                         await ctx.CreateResponseAsync("There was an error getting the status code");
                    }
                    else
                    {
                         await ctx.CreateResponseAsync(response.StatusCode.ToString());
                    }
               }
               
          }
          catch (Exception e)
          {
               await ctx.CreateResponseAsync(e.Message);
          }
     }
}