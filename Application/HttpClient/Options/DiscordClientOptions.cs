using Interface.Options;

namespace Application.HttpClient.Options;

public class DiscordClientOptions : IOptionsImpl
{
    public static string SectionName => "Discord";
    
    public required Uri WebhookUrl { get; init; }
}