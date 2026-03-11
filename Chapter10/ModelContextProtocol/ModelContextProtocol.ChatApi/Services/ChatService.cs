using System.Diagnostics;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Domain.Requests;
using ModelContextProtocol.Protocol;

namespace ModelContextProtocol.ChatApi.Services;

public class ChatService(IOptionsSnapshot<ChatSettings> settings, ILoggerFactory logFactory, IChatClient chatClient, IList<AITool> tools) : IChatService
{
    private readonly ActivitySource _activitySource = new(typeof(ChatService).Assembly.FullName!);
    private readonly ILogger<ChatService> _logger = logFactory.CreateLogger<ChatService>();

    public async IAsyncEnumerable<string> ChatAsync(ChatRequest request)
    {
        ChatSettings options = settings.Value;
        List<ChatMessage> messages = BuildChatMessages(request, options.SystemPrompt);

        _logger.LogTrace("Creating chat with model {id} at {endpoint}", options.ChatModelId, options.ChatEndpoint);

        ChatOptions chatOptions = new() { Tools = tools };

        ChatResponse response;
        using (_activitySource.StartActivity(ActivityKind.Client))
        {
            response = await chatClient.GetResponseAsync(messages, chatOptions);
        }

        int index = 1;
        foreach (var message in response.Messages)
        {
            if (!string.IsNullOrWhiteSpace(message.Text))
            {
                _logger.LogTrace("Response {index}: {content}", index, message.Text);
                yield return message.Text;
            }
            index++;
        }
    }

    private List<ChatMessage> BuildChatMessages(ChatRequest request, string sysPrompt)
    {
        using Activity? activity = _activitySource.StartActivity(ActivityKind.Server);
        List<ChatMessage> messages = [new(ChatRole.System, sysPrompt)];

        int index = 1;
        foreach (var entry in request.Messages)
        {
            activity?.AddEvent(new ActivityEvent($"{entry.Role}-{index++}: {entry.Message}"));
            _logger.LogTrace("{Role}: {Message}", entry.Role, entry.Message);
            if (entry.Role == Role.Assistant)
            {
                messages.Add(new ChatMessage(ChatRole.Assistant, entry.Message));
            }
            else
            {
                messages.Add(new ChatMessage(ChatRole.User, entry.Message));
            }
        }

        return messages;
    }
}
