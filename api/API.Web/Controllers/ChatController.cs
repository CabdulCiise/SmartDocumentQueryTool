using API.Core.DTOs;
using API.Core.Models;
using API.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Web.Controllers;

public class ChatController(IChatService chatService) : BaseApiController
{
    [HttpGet("User/{userId}")]
    public List<Chat> GetChats(int userId)
    {
        return chatService.GetChats(userId);
    }

    [HttpGet("{chatId}/messages")]
    public List<ChatMessageDto> GetChatHistory(int chatId)
    {
        return chatService.GetChatHistory(chatId);
    }

    [HttpGet("{chatId}/user/{userId}/{prompt}")]
    public async Task GetResponse(int chatId, int userId, string prompt)
    {
        await chatService.GetResponse(chatId, userId, prompt, Response.Body);
    }

    [HttpDelete("{chatId}")]
    public void DeleteChat(int chatId)
    {
        chatService.DeleteChat(chatId);
    }
}
