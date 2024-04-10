namespace API.Data.Entities;

public class DocumentEmbedding : BaseEntity
{
    public string EmbeddingId { get; set; }
    public int UploadedDocumentId { get; set; }

    public virtual Document UploadedDocument { get; set; }
}
