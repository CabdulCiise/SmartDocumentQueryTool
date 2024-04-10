using API.Core.Models;
using API.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Web.Controllers;

public class DocumentController(IDocumentService documentService) : BaseApiController
{
    [HttpGet]
    public List<Document> GetDocuments(int userId)
    {
        return documentService.GetDocuments(userId);
    }

    [HttpDelete("{id}")]
    public async Task DeleteDocument(int id)
    {
        await documentService.DeleteDocument(id);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDocument([FromForm]int userId, IFormFile file)
    {
        try
        {
            return Ok(await documentService.CreateDocument(userId, file.FileName, file.OpenReadStream()));

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
