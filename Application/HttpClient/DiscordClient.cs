using System.Text;
using System.Text.Json;
using Application.HttpClient.Options;
using Interface.Discord;
using Microsoft.Extensions.Options;

namespace Application.HttpClient;

public class DiscordClient(
    System.Net.Http.HttpClient httpClient,
    IOptions<DiscordClientOptions> options) : IDiscordClient
{
    public async Task<HttpResponseMessage> SendMessageAsync(
        DiscordWebHookMessage message,
        CancellationToken cancellationToken = default)
    {
        var content = new StringContent(
            JsonSerializer.Serialize(message), 
            Encoding.UTF8,
            "application/json");
        
        return await httpClient.PostAsync(options.Value.WebhookUrl, content, cancellationToken);
    }

    public async Task<HttpResponseMessage> SendSimpleMessageAsync(
        string content,
        string? username = null,
        string? avatarUrl = null,
        CancellationToken cancellationToken = default)
    {
        var message = new DiscordWebHookMessage
        {
            Content = content,
            Username = username,
            AvatarUrl = avatarUrl,
        };
        
        return await this.SendMessageAsync(message, cancellationToken);
    }

    public async Task<HttpResponseMessage> SendEmbedMessageAsync(
        string title,
        string description,
        int color = 0,
        string? username = null,
        string? avatarUrl = null,
        CancellationToken cancellationToken = default)
    {
        var message = new DiscordWebHookMessage
        {
            Username = username,
            AvatarUrl = avatarUrl,
            Embeds =
            [
                new DiscordEmbed
                {
                    Title = title,
                    Description = description,
                    Color = color,
                    Timestamp = DateTime.UtcNow.ToString("o"),
                },
            ],
        };
        
        return await this.SendMessageAsync(message, cancellationToken);
    }
}