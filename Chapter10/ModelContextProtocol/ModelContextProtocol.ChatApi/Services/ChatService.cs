using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Ollama;
using ModelContextProtocol.Domain.Requests;
using ModelContextProtocol.Protocol;

namespace ModelContextProtocol.ChatApi.Services;

public class ChatService(IOptionsSnapshot<ChatSettings> settings, ILoggerFactory logFactory, Kernel kernel) : IChatService
{
    private readonly ActivitySource _activitySource = new(typeof(ChatService).Assembly.FullName!);
    private readonly ILogger<ChatService> _logger = logFactory.CreateLogger<ChatService>();

    [Experimental("SKEXP0070")]
    public async IAsyncEnumerable<string> ChatAsync(ChatRequest request)
    {
        ChatSettings options = settings.Value;
        string sysPrompt = options.SystemPrompt;
        ChatHistory history = BuildChatHistory(request, sysPrompt);
        
        _logger.LogTrace("Creating kernel with chat model {id} at {endpoint}", options.ChatModelId, options.ChatEndpoint);

        IChatCompletionService completions = kernel.GetRequiredService<IChatCompletionService>();
        IReadOnlyList<ChatMessageContent> responses;
        using (_activitySource.StartActivity(ActivityKind.Client))
        {
            OllamaPromptExecutionSettings execSettings = new()
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            };
            responses = await completions.GetChatMessageContentsAsync(history, execSettings, kernel);
        }

        int index = 1;
        foreach (var response in responses)
        {
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                _logger.LogTrace("Response {index}: {content}", index, response.Content);
                yield return response.Content;
            }
            index++;
        }
    }

    private ChatHistory BuildChatHistory(ChatRequest request, string sysPrompt)
    {
        ChatHistory history = new(sysPrompt);
        using Activity? activity = _activitySource.StartActivity(ActivityKind.Server);
        
        int index = 1;
        foreach (var entry in request.Messages)
        {
            activity?.AddEvent(new ActivityEvent($"{entry.Role}-{index++} Assistant: {entry.Message}"));
            _logger.LogTrace("{Role}: {Message}", entry.Role, entry.Message);
            if (entry.Role == Role.Assistant)
            {
                history.AddAssistantMessage(entry.Message);
            }
            else
            {
                history.AddUserMessage(entry.Message);
            }
        }

        return history;
    }
}
