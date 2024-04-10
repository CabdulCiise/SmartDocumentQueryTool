namespace API.Data.Entities;

public class ChatMessage : BaseEntity
{
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsUserMessage { get; set; }
    public int ChatId { get; set; }
    public double? Cost { get; set; }
    public TimeSpan? ProcessTime { get; set; }

    public virtual Chat Chat { get; set; }
}
