using API.Core.Models;
using API.Core.Services;
using API.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Moq;

namespace API.Test.Integration.Controllers;

[TestFixture]
public class DocumentControllerTests
{
    private DocumentController documentController;
    private Mock<IDocumentService> mockDocumentService;

    public DocumentControllerTests()
    {
        mockDocumentService = new Mock<IDocumentService>();
        documentController = new DocumentController(mockDocumentService.Object);
    }

    [Test]
    public void GetDocuments_CallGetDocumentsInService()
    {
        int userId = 123;
        var expectedDocuments = new List<Document>();

        mockDocumentService.Setup(x => x.GetDocuments(userId)).Returns(expectedDocuments);

        var result = documentController.GetDocuments(userId);

        mockDocumentService.Verify(x => x.GetDocuments(userId), Times.Once);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<List<Document>>(result);
        Assert.That(result, Is.EqualTo(expectedDocuments));
    }

    [Test]
    public async Task DeleteDocument_CallDeleteDocumentInService()
    {
        int documentId = 456;

        await documentController.DeleteDocument(documentId);

        mockDocumentService.Verify(x => x.DeleteDocument(documentId), Times.Once);
    }

    [Test]
    public async Task CreateDocument_CallCreateDocumentInService()
    {
        int userId = 789;
        var file = new Mock<IFormFile>().Object;

        var result = await documentController.CreateDocument(userId, file);

        mockDocumentService.Verify(x => x.CreateDocument(userId, file.FileName, file.OpenReadStream()), Times.Once);
    }
}
