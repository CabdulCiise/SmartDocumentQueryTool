namespace API.Data.Entities;

public class Chat : BaseEntity
{
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }
    public bool IsDeleted { get; set; }

    public virtual User User { get; set; }
    public virtual ICollection<ChatMessage> Messages { get; set; }
}
