using System.Diagnostics;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Domain.Requests;
using ModelContextProtocol.Protocol;

namespace ModelContextProtocol.ChatApi.Services;

public class ChatService(IOptionsSnapshot<ChatSettings> settings, ILoggerFactory logFactory, AIAgent agent) : IChatService
{
    private readonly ActivitySource _activitySource = new(typeof(ChatService).Assembly.FullName!);
    private readonly ILogger<ChatService> _logger = logFactory.CreateLogger<ChatService>();

    public async IAsyncEnumerable<string> ChatAsync(ChatRequest request)
    {
        ChatSettings options = settings.Value;
        List<ChatMessage> messages = BuildChatMessages(request);

        _logger.LogTrace("Running agent with model {id} at {endpoint}", options.ChatModelId, options.ChatEndpoint);

        AgentResponse response;
        using (Activity? activity = _activitySource.StartActivity("ChatAsync", ActivityKind.Client))
        {
            activity?.SetTag("gen_ai.request.model", options.ChatModelId);
            activity?.SetTag("chat.message_count", messages.Count);

            AgentSession session = await agent.CreateSessionAsync();
            response = await agent.RunAsync(messages, session);

            if (response.Usage is { } usage)
            {
                activity?.SetTag("gen_ai.usage.input_tokens", usage.InputTokenCount);
                activity?.SetTag("gen_ai.usage.output_tokens", usage.OutputTokenCount);
                activity?.SetTag("gen_ai.usage.total_tokens", usage.TotalTokenCount);
            }
        }

        if (!string.IsNullOrWhiteSpace(response.Text))
        {
            _logger.LogTrace("Response: {content}", response.Text);
            yield return response.Text;
        }
    }

    private List<ChatMessage> BuildChatMessages(ChatRequest request)
    {
        using Activity? activity = _activitySource.StartActivity("BuildChatMessages", ActivityKind.Server);
        List<ChatMessage> messages = [];

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
