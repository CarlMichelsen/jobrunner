using System.Text.Json.Serialization;

namespace Interface.Discord;

public class DiscordEmbed
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    [JsonPropertyName("url")]
    public string? Url { get; set; }
    
    [JsonPropertyName("color")]
    public int? Color { get; set; }
    
    [JsonPropertyName("footer")]
    public DiscordEmbedFooter? Footer { get; set; }
    
    [JsonPropertyName("timestamp")]
    public string? Timestamp { get; set; }
}