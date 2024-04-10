namespace API.Data.Entities;

public class Document : BaseEntity
{
    public string Name { get; set; }
    public DateTime UploadedAt { get; set; }
    public TimeSpan ProcessTime { get; set; }
    public double Cost { get; set; }
    public int UserId { get; set; }

    public virtual User User { get; set; }
    public virtual ICollection<DocumentEmbedding> Embeddings { get; set; }
}
