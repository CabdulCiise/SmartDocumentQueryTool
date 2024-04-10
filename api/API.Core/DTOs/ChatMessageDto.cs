namespace API.Core.DTOs;

public class ChatMessageDto
{
    public int ChatId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Prompt { get; set; }
    public string? Response { get; set; }
    public double? Cost { get; set; }
    public TimeSpan? ProcessTime { get; set; }
}
