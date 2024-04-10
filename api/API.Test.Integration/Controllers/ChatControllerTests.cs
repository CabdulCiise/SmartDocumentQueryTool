using API.Core.DTOs;
using API.Core.Models;
using API.Core.Services;
using API.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API.Test.Integration.Controllers;

[TestFixture]
public class ChatControllerTests
{
    private ChatController ChatController;
    private Mock<IChatService> MockChatService;

    public ChatControllerTests()
    {
        MockChatService = new Mock<IChatService>();
        ChatController = new ChatController(MockChatService.Object);

        // Mock the HttpContext
        var httpContextMock = new Mock<HttpContext>();
        var responseMock = new Mock<HttpResponse>();
        responseMock.SetupGet(r => r.Body).Returns(new MemoryStream());
        httpContextMock.SetupGet(c => c.Response).Returns(responseMock.Object);

        ChatController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContextMock.Object
        };
    }

    [Test]
    public async Task GetResponse_CallsGetResponseInService()
    {
        int chatId = 123;
        int userId = 456;
        string prompt = "Test Prompt";

        await ChatController.GetResponse(chatId, userId, prompt);

        MockChatService.Verify(x => x.GetResponse(chatId, userId, prompt, It.IsAny<Stream>()), Times.Once);
    }

    [Test]
    public void GetChats_CallGetChatsInService()
    {
        int userId = 123;
        var expectedChats = new List<Chat>();

        MockChatService.Setup(x => x.GetChats(userId)).Returns(expectedChats);

        var result = ChatController.GetChats(userId);

        MockChatService.Verify(x => x.GetChats(userId), Times.Once);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<List<Chat>>(result);
        Assert.That(result, Is.EqualTo(expectedChats));
    }

    [Test]
    public void GetChatHistory_CallGetChatHistoryInService()
    {
        int chatId = 456;
        var expectedChatMessages = new List<ChatMessageDto>();

        MockChatService.Setup(x => x.GetChatHistory(chatId)).Returns(expectedChatMessages);
        var result = ChatController.GetChatHistory(chatId);

        MockChatService.Verify(x => x.GetChatHistory(chatId), Times.Once);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<List<ChatMessageDto>>(result);
        Assert.That(result, Is.EqualTo(expectedChatMessages));
    }

    [Test]
    public void DeleteChat_CallsDeleteChatInService()
    {
        int chatId = 456;

        ChatController.DeleteChat(chatId);

        MockChatService.Verify(x => x.DeleteChat(chatId), Times.Once);
    }
}