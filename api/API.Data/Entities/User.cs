namespace API.Data.Entities;

public class User : BaseEntity
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string CustomInstruction { get; set; }
    public DateTime CreatedAt { get; set; }
    public int RoleId { get; set; }

    public virtual Role Role { get; set; }
    public virtual ICollection<Chat> Chats { get; set; }
    public virtual ICollection<Feedback> Feedbacks { get; set; }
    public virtual ICollection<Document> Documents { get; set; }
}
