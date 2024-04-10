using API.Core.DTOs;
using API.Core.Models;
using API.Core.Services;
using API.Data.Contexts;
using AutoMapper;
using LangChain.Providers;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Services;

public class ChatService(ApiContext apiContext, IOpenApiService openApiService, IMapper mapper) : IChatService
{
    public List<Chat> GetChats(int userId)
    {
        return mapper.Map<List<Chat>>(apiContext.Chats
            .Where(x => !x.IsDeleted && x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToList()
        );
    }

    public void DeleteChat(int chatId)
    {
        var chat = apiContext.Chats
            .Include(x => x.Messages)
            .FirstOrDefault(x => x.Id == chatId);

        if (chat != null)
        {
            apiContext.ChatMessages.RemoveRange(chat.Messages);
            apiContext.Chats.Remove(chat);
            apiContext.SaveChanges();
        }
    }

    public List<ChatMessageDto> GetChatHistory(int chatId)
    {
        var entities = apiContext.ChatMessages
            .Where(x => x.ChatId == chatId)
            .OrderBy(x => x.CreatedAt)
            .ToList();

        var messages = new List<ChatMessageDto>();

        for (int i = 0; i < entities.Count; i++)
        {
            var entity = entities[i];

            if (!entity.IsUserMessage)
            {
                continue;
            }

            var nextEntity = i + 1 < entities.Count ? entities[i + 1] : null;

            var message = new ChatMessageDto
            {
                Prompt = entity.Message,
                Response = nextEntity != null && !nextEntity.IsUserMessage ? nextEntity?.Message : null,
                CreatedAt = entity.CreatedAt,
                Cost = nextEntity != null && !nextEntity.IsUserMessage ? nextEntity?.Cost : null,
                ProcessTime = nextEntity != null && !nextEntity.IsUserMessage ? nextEntity?.ProcessTime : null,
                ChatId = entity.ChatId,
            };

            messages.Add(message);
        }

        return messages;
    }

    public async Task GetResponse(int chatId, int userId, string prompt, Stream responseStream)
    {
        var chat = apiContext.Chats.FirstOrDefault(x => x.Id == chatId);

        if (chat == null)
        {
            var (title, titleUsage) = await openApiService.GetChatTitle(prompt);

            chat = new Data.Entities.Chat
            {
                Id = chatId,
                CreatedAt = DateTime.Now,
                Name = title,
                UserId = userId,
                IsDeleted = false,
            };

            apiContext.Chats.Add(chat);
            apiContext.SaveChanges();
        }

        SaveUserChatMessage(chat.Id, prompt, true);

        var user = apiContext.Users.FirstOrDefault(x => x.Id == userId);
        var chatMessages = apiContext.ChatMessages.Where(x => x.ChatId == chat.Id).OrderBy(x => x.CreatedAt).ToList();

        var messages = chatMessages
            .Select(x => new Message { Content = x.Message, Role = x.IsUserMessage ? MessageRole.Human : MessageRole.Ai })
            .ToList();

        var (response, usage) = await openApiService.GetResponse(userId, prompt, user?.CustomInstruction ?? "", messages, responseStream);

        SaveAiChatMessage(chat.Id, response, false, usage);
    }

    private void SaveUserChatMessage(int chatId, string message, bool isUserMessage)
    {
        var chatMessage = new Data.Entities.ChatMessage
        {
            ChatId = chatId,
            Message = message,
            IsUserMessage = isUserMessage,
            CreatedAt = DateTime.Now,
            Cost = null,
            ProcessTime = null
        };

        apiContext.ChatMessages.Add(chatMessage);
        apiContext.SaveChanges();
    }

    private void SaveAiChatMessage(int chatId, string message, bool isUserMessage, Usage usage)
    {
        var chatMessage = new Data.Entities.ChatMessage
        {
            ChatId = chatId,
            Message = message,
            IsUserMessage = isUserMessage,
            CreatedAt = DateTime.Now,
            Cost = usage.PriceInUsd,
            ProcessTime = usage.Time
        };

        apiContext.ChatMessages.Add(chatMessage);
        apiContext.SaveChanges();
    }
}
