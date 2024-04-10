using API.Core.DTOs;
using API.Core.Models;

namespace API.Core.Services;

public interface IChatService
{
    List<Chat> GetChats(int userId);
    void DeleteChat(int chatId);
    List<ChatMessageDto> GetChatHistory(int chatId);
    Task GetResponse(int chatId, int userId, string prompt, Stream responseStream);
}
