using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Ollama;
using ModelContextProtocol.Client;
using ModelContextProtocol.Domain.Requests;
using ModelContextProtocol.Protocol;

namespace ModelContextProtocol.ChatApi.Services;

public class ChatService(IOptionsSnapshot<ChatSettings> settings, ILoggerFactory logFactory) : IChatService
{
    private readonly ActivitySource _activitySource = new(typeof(ChatService).Assembly.FullName!);
    private readonly ILogger<ChatService> _logger = logFactory.CreateLogger<ChatService>();
    private IMcpClient? _mcpClient;

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

        _logger.LogTrace("Creating kernel with chat model {id} at {endpoint}", options.ChatModelId, options.ChatEndpoint);

        IKernelBuilder builder = Kernel.CreateBuilder()
            .AddOllamaChatCompletion(options.ChatModelId, new Uri(options.ChatEndpoint));
        builder.Services.AddSingleton(logFactory);
            
        Kernel kernel = builder.Build();
        // TODO: Loading the MCP Server and kernel with every request is inefficient
        await AddMcpToolsAsync(options, kernel);

        using HttpClient client = new();
        client.Timeout = TimeSpan.FromSeconds(60);

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

    [Experimental("SKEXP0001")]
    private async Task AddMcpToolsAsync(ChatSettings options, Kernel kernel)
    {
        _logger.LogTrace("Connecting to MCP Server at {endpoint}", options.McpServerEndpoint);
        IClientTransport clientTransport = new SseClientTransport(new()
        {
            Name = "Custom MCP Server",
            Endpoint = new Uri(options.McpServerEndpoint),
            UseStreamableHttp = true
        });
        _mcpClient = await McpClientFactory.CreateAsync(clientTransport);

        await foreach (var tool in _mcpClient.EnumerateToolsAsync())
        {
            _logger.LogTrace("Registering MCP Tool {Name}", tool.Name);
            kernel.Plugins.AddFromFunctions(tool.Name, tool.Description, [tool.AsKernelFunction()]);
        }
    }

    private ChatHistory BuildChatHistory(ChatRequest request, string sysPrompt)
    {
        using Activity? activity = _activitySource.StartActivity(ActivityKind.Server);
        activity?.AddTag("History Size", request.Messages.Count());

        ChatHistory history = new(sysPrompt);

        int index = 1;
        foreach (var entry in request.Messages)
        {
            if (entry.Role == Role.Assistant)
            {
                activity?.AddEvent(new ActivityEvent($"History-{index++} Assistant: {entry.Message}"));
                _logger.LogTrace("Assistant: {Message}", entry.Message);
                history.AddAssistantMessage(entry.Message);
            }
            else
            {
                activity?.AddEvent(new ActivityEvent($"User-{index++} Assistant: {entry.Message}"));
                _logger.LogTrace("User: {Message}", entry.Message);
                history.AddUserMessage(entry.Message);
            }
        }

        return history;
    }
}
