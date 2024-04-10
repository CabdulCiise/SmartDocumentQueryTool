using LangChain.Providers;

namespace API.Core.Services;

public interface IOpenApiService
{
    Task DeleteEmbeddedings(int userId, List<string> documentIds);
    Task<(IEnumerable<string>, Usage)> ProcessNewFile(int userId, Stream file);
    Task<(string, Usage)> GetResponse(int userId, string prompt, string customInstruction, List<Message> messages, Stream responseStream);
    Task<(string, Usage)> GetChatTitle(string prompt);
}
