using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Ollama;
using ModelContextProtocol.ChatApi.Requests;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.SemanticKernel.Extensions;
using ModelContextProtocol.SemanticKernel.Options;
using OllamaSharp;

namespace ModelContextProtocol.ChatApi.Services;

public class ChatService(IOptionsSnapshot<ChatSettings> settings, ILoggerFactory logFactory) : IChatService
{
    private readonly ILogger<ChatService> _logger = logFactory.CreateLogger<ChatService>();

    [Experimental("SKEXP0070")]
    public async IAsyncEnumerable<string> ChatAsync(ChatRequest request)
    {
        ChatSettings options = settings.Value;
        string sysPrompt = options.SystemPrompt;
        ChatHistory history = BuildChatHistory(request, sysPrompt);

        OllamaPromptExecutionSettings execSettings = new()
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };

        // TODO: Loading the MCP Server and kernel with every request is inefficient
        _logger.LogTrace("Creating kernel with chat model {id} at {endpoint}", options.ChatModelId, options.ChatEndpoint);

        IKernelBuilder builder = Kernel.CreateBuilder()
            .AddOllamaChatCompletion(options.ChatModelId, new Uri(options.ChatEndpoint));
        builder.Services.AddSingleton(logFactory);
            
        Kernel kernel = builder.Build();

        _logger.LogTrace("Connecting to MCP Server at {endpoint}", options.McpServerEndpoint);
        ModelContextProtocolSemanticKernelSseOptions sseOptions = new()
        {
            Name = "Custom MCP Server",
            Endpoint = new Uri(options.McpServerEndpoint),
            LoggerFactory = logFactory
        };

        using HttpClient client = new();
        client.Timeout = TimeSpan.FromSeconds(60);

        await kernel.Plugins.AddMcpFunctionsFromSseServerAsync(sseOptions, client);

        IChatCompletionService completions = kernel.GetRequiredService<IChatCompletionService>();

        _logger.LogTrace("Getting responses");
        IReadOnlyList<ChatMessageContent> responses = await completions.GetChatMessageContentsAsync(history, execSettings, kernel);

        int index = 0;
        foreach (var response in responses)
        {
            index++;

            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                _logger.LogTrace("Response {index}: {content}", index, response.Content);
                yield return response.Content;
            }
        }
    }

    private ChatHistory BuildChatHistory(ChatRequest request, string sysPrompt)
    {
        _logger.LogTrace("Building conversation history");
        ChatHistory history = new(sysPrompt);

        foreach (var entry in request.Messages)
        {
            if (entry.Role == Role.Assistant)
            {
                _logger.LogTrace("Assistant: {Message}", entry.Message);
                history.AddAssistantMessage(entry.Message);
            }
            else
            {
                _logger.LogTrace("User: {Message}", entry.Message);
                history.AddUserMessage(entry.Message);
            }
        }

        return history;
    }
}
