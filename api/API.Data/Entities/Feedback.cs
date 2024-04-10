namespace API.Data.Entities;

public class Feedback : BaseEntity
{
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsArchived { get; set; }
    public int UserId { get; set; }

    public virtual User User { get; set; }
}
