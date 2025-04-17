using System.Text.Json.Serialization;

namespace Interface.Discord;

public class DiscordWebHookMessage
{
    [JsonPropertyName("content")]
    public string? Content { get; set; }
    
    [JsonPropertyName("username")]
    public string? Username { get; set; }
    
    [JsonPropertyName("avatar_url")]
    public string? AvatarUrl { get; set; }
    
    [JsonPropertyName("embeds")]
    public List<DiscordEmbed>? Embeds { get; set; }
    
    [JsonPropertyName("tts")]
    public bool? Tts { get; set; }
}