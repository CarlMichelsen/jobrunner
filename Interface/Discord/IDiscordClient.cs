namespace Interface.Discord;

public interface IDiscordClient
{
    Task<HttpResponseMessage> SendMessageAsync(
        DiscordWebHookMessage message,
        CancellationToken cancellationToken = default);
    
    Task<HttpResponseMessage> SendSimpleMessageAsync(
        string content,
        string? username = null,
        string? avatarUrl = null,
        CancellationToken cancellationToken = default);

    Task<HttpResponseMessage> SendEmbedMessageAsync(
        string title,
        string description,
        int color = 0,
        string? username = null,
        string? avatarUrl = null,
        CancellationToken cancellationToken = default);
}