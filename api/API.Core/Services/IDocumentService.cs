using API.Core.Models;

namespace API.Core.Services;

public interface IDocumentService
{
    List<Document> GetDocuments(int userId);
    Task DeleteDocument(int id);
    Task<Document> CreateDocument(int userId, string fileName, Stream file);
}
