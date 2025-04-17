using System.Text.Json.Serialization;

namespace Interface.Discord;

public class DiscordEmbedFooter
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }
    
    [JsonPropertyName("icon_url")]
    public string? IconUrl { get; set; }
}