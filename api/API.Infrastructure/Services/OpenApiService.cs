using LangChain.Databases;
using LangChain.Extensions;
using LangChain.Providers.OpenAI.Predefined;
using LangChain.Providers.OpenAI;
using LangChain.Sources;
using LangChain.Splitters.Text;
using Microsoft.Extensions.Configuration;
using API.Core.Services;
using LangChain.Providers;
using LangChain.VectorStores;
using System.Text;

namespace API.Infrastructure.Services;

public class OpenApiService : IOpenApiService
{
    private string openApiKey;
    private RecursiveCharacterTextSplitter textSplitter;
    private SQLIteVectorStoreOptions vectorStoreOptions;

    public OpenApiService(IConfiguration configuration)
    {
        openApiKey = configuration["OpenApiKey"];

        vectorStoreOptions = new SQLIteVectorStoreOptions { Filename = "localDatabase.db", TableName = "vectors", ChunkSize = 200, ChunkOverlap = 50 };
        textSplitter = new RecursiveCharacterTextSplitter(
            chunkSize: vectorStoreOptions.ChunkSize,
            chunkOverlap: vectorStoreOptions.ChunkOverlap);
    }

    public async Task DeleteEmbeddedings(int userId, List<string> documentIds)
    {
        vectorStoreOptions.TableName = $"{vectorStoreOptions.TableName}_{userId}";

        var provider = new OpenAiProvider(openApiKey);
        var embeddings = new TextEmbeddingV3SmallModel(provider);
        var vectorStore = new SQLiteVectorStore(vectorStoreOptions.Filename, vectorStoreOptions.TableName, embeddings);

        await vectorStore.DeleteAsync(documentIds);
    }

    public async Task<(IEnumerable<string>, Usage)> ProcessNewFile(int userId, Stream file)
    {
        var provider = new OpenAiProvider(openApiKey);
        var embeddings = new TextEmbeddingV3SmallModel(provider);

        vectorStoreOptions.TableName = "vectors_" + userId;

        var vectorStore = new SQLiteVectorStore(vectorStoreOptions.Filename, vectorStoreOptions.TableName, embeddings);
        var documents = await PdfPigPdfSource.FromStreamAsync(file);

        var subDocs = textSplitter.SplitDocuments(documents);

        var documentIds = await vectorStore.AddDocumentsAsync(subDocs);

        var usage = embeddings.Usage;

        return (documentIds, usage);
    }

    public async Task<(string, Usage)> GetResponse(int userId, string prompt, string customInstruction, List<Message> messages, Stream responseStream)
    {
        var provider = new OpenAiProvider(openApiKey);
        var embeddings = new TextEmbeddingV3SmallModel(provider);
        var vectorStore = new SQLiteVectorStore(vectorStoreOptions.Filename, $"{vectorStoreOptions.TableName}_{userId}", embeddings);
        var similarDocuments = await vectorStore.GetSimilarDocuments(prompt, amount: 5);

        var llm = new Gpt35TurboModel(provider);
        llm.PartialResponseGenerated += async (sender, args) =>
        {
            var responseBytes = Encoding.UTF8.GetBytes(args ?? string.Empty);
            await responseStream.WriteAsync(responseBytes, 0, responseBytes.Length);
            await responseStream.FlushAsync();
        };

        if (similarDocuments.Count != 0)
        {
            var message = new Message
            {
                Content =
                $"""
                Use ONLY the following pieces of context to answer. If there's no context, just simply say "I don't know" and don't try to make up an answer.
                If the answer is not in context then simply just say "I don't know", don't try to make up an answer.
                Do not use any other context than the following text even if the user asks for it. If the user asks for more context, just say "I don't know".
            
                {customInstruction}

                Context: {similarDocuments.AsString()}
                """,
                Role = MessageRole.System
            };

            messages = messages.Prepend(message).ToList();

            var chat = new ChatRequest() { Messages = messages };
            var settings = new ChatSettings { StopSequences = null, User = string.Empty, UseStreaming = true };
            var chatResponse = await llm.GenerateAsync(chat, settings);

            var usage = llm.Usage;

            return (chatResponse.LastMessageContent, usage);
        }
        else
        {
            var responseBytes = Encoding.UTF8.GetBytes("I don't know." ?? string.Empty);
            await responseStream.WriteAsync(responseBytes, 0, responseBytes.Length);
            await responseStream.FlushAsync();

            return ("I don't know.", new Usage());
        }
    }

    public async Task<(string, Usage)> GetChatTitle(string prompt)
    {
        var provider = new OpenAiProvider(openApiKey);
        var llm = new Gpt35TurboModel(provider);

        var message = new Message
        {
            Content =
            $"""
                 Come up with a few words to describe the following text. This will be used as a title.
                 
                 {prompt}
            """,
            Role = MessageRole.System
        };

        var chat = new ChatRequest() { Messages = new List<Message> { message } };
        var settings = new ChatSettings { StopSequences = null, User = string.Empty, UseStreaming = false };
        var chatResponse = await llm.GenerateAsync(chat, settings);

        return (chatResponse.LastMessageContent, llm.Usage);
    }
}
