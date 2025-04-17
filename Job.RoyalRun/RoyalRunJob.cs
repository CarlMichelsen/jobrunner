using Browser;
using Interface.Discord;
using Interface.Job;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;

namespace Job.RoyalRun;

public class RoyalRunJob(
    ILogger<RoyalRunJob> logger,
    IMemoryCache cache,
    IDiscordClient client,
    BrowserHandler browserHandler) : IGenericJob
{
    private const string NoTicketString = "Der findes ingen billetter til salg";
    private const string RoyalRunTicketPage = "https://www.sportstiming.dk/event/15228/resale";
    
    public async Task Run(CancellationToken stoppingToken)
    {
        await browserHandler.Connect(
            async (webDriver, ctx) =>
            {
                await webDriver
                    .Navigate()
                    .GoToUrlAsync(RoyalRunTicketPage);
                
                await Task.Delay(TimeSpan.FromSeconds(2), ctx);
                webDriver
                    .FindElement(By.Id("btnCookiesAcceptAll"))
                    .Click();
                
                await Task.Delay(TimeSpan.FromSeconds(2), ctx);

                var panelBody = webDriver
                    .FindElement(By.ClassName("panel-body"));

                var ticketString = panelBody
                    .FindElement(By.TagName("p"))
                    .Text;

                if (ticketString != NoTicketString)
                {
                    var notificationHash = GetSha256Hash(panelBody.Text);
                    await this.HandleTicketsFound(notificationHash);
                    return true;
                }
                
                logger.LogInformation("No tickets for sale :(");
                return false;
            },
            stoppingToken);
    }
    
    private static string GetSha256Hash(string input)
    {
        var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
        var hashBytes = System.Security.Cryptography.SHA256.HashData(inputBytes);
        return Convert.ToHexString(hashBytes).ToLower();
    }

    private async Task HandleTicketsFound(string notificationHash)
    {
        if (cache.TryGetValue(notificationHash, out bool notified) && notified)
        {
            return;
        }

        try
        {
            await client.SendSimpleMessageAsync(
                $"There are RoyalRun tickets available\n{RoyalRunTicketPage}",
                "Royal Run Ticket Notification",
                null,
                CancellationToken.None);
            
            cache.Set(
                notificationHash,
                true, 
                TimeSpan.FromHours(12));
        }
        catch (Exception e)
        {
            cache.Set(
                notificationHash,
                false, 
                TimeSpan.FromHours(12));
            
            logger.LogError(e, "Could not send Royal Run Ticket Notification");
        }
    }
}
