namespace API.Core.Models;

public class Chat
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }

    public User User { get; set; }
    public List<ChatMessage> Messages { get; set; }
}
