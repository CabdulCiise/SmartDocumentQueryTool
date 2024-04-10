namespace API.Core.Models;

public class ChatMessage
{
    public int Id { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsUserMessage { get; set; }
    public int ChatId { get; set; }
    public double? Cost { get; set; }
    public TimeSpan? ProcessTime { get; set; }

    public Chat Chat { get; set; }
}
