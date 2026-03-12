using System.Text.Json.Serialization;

namespace ChatServer;

public sealed class Packet
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "";

    [JsonPropertyName("login")]
    public string? Login { get; set; }

    [JsonPropertyName("from")]
    public string? From { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTime? Timestamp { get; set; }

    [JsonPropertyName("users")]
    public List<string>? Users { get; set; }

    [JsonPropertyName("messages")]
    public List<ChatMessage>? Messages { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }
}

public sealed class ChatMessage
{
    [JsonPropertyName("from")]
    public string From { get; set; } = "";

    [JsonPropertyName("text")]
    public string Text { get; set; } = "";

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
}