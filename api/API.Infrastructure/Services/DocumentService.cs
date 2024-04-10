using API.Core.Models;
using API.Core.Services;
using API.Data.Contexts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;


namespace API.Infrastructure.Services;

public class DocumentService(ApiContext apiContext, IMapper mapper, IOpenApiService openApiService) : IDocumentService
{
    public List<Document> GetDocuments(int userId)
    {
        return mapper.Map<List<Document>>(apiContext.Documents
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.UploadedAt)
            .ToList());
    }

    public async Task DeleteDocument(int id)
    {
        var entity = apiContext.Documents
            .Include(x => x.Embeddings)
            .FirstOrDefault(x => x.Id == id);

        if (entity != null)
        {
            await openApiService.DeleteEmbeddedings(entity.UserId, entity.Embeddings.Select(x => x.EmbeddingId).ToList());

            apiContext.Documents.Remove(entity);
            apiContext.SaveChanges();
        }
    }

    public async Task<Document> CreateDocument(int userId, string fileName, Stream file)
    {
        var entity = apiContext.Documents.FirstOrDefault(x => x.UserId == userId && x.Name == fileName);

        if (entity != null)
        {
            throw new Exception("Document with the same name has already been uploaded.");
        }

        var (embeddingIds, usage) = await openApiService.ProcessNewFile(userId, file);

        entity = new Data.Entities.Document
        {
            UserId = userId,
            Cost = usage.PriceInUsd,
            ProcessTime = usage.Time,
            UploadedAt = DateTime.Now,
            Name = fileName,
            Embeddings = embeddingIds.Select(x => new Data.Entities.DocumentEmbedding { EmbeddingId = x }).ToList(),
        };

        apiContext.Documents.Add(entity);
        apiContext.SaveChanges();

        return mapper.Map<Document>(entity);
    }
}
