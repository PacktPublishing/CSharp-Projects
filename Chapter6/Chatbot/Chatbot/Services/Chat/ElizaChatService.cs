using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;

namespace Chatbot.Services.Chat;
public class ElizaChatService : IChatClient
{
    public string Chat(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return "Please tell me more.";
        }

        // Define some basic patterns and responses
        Dictionary<string, string> patterns = new()
        {
            { @"\bI need (.*)", "Why do you need {0}?" },
            { @"\bI am (.*)", "How long have you been {0}?" },
            { @"\bI feel (.*)", "Why do you feel {0}?" },
            { @"\bBecause (.*)", "Is that the real reason?" },
            { @"\bWhy don't you (.*)", "What makes you think I don't {0}?" },
            { @"\bWhy can't I (.*)", "What makes you think you can't {0}?" },
            { @"\bI can't (.*)", "What would it take for you to {0}?" },
            { @"\bI think (.*)", "Why do you think {0}?" },
            { @"\bI want (.*)", "What would it mean if you got {0}?" },
            { @"\b(.*) mother(.*)", "Tell me more about your mother." },
            { @"\b(.*) father(.*)", "Tell me more about your father." },
            { @"\b(.*)\?", "What do you think?" },
        };

        // Iterate through patterns and find a match
        foreach (var pattern in patterns)
        {
            Match match = Regex.Match(input, pattern.Key, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                // Replace placeholders with matched groups
                return string.Format(pattern.Value, match.Groups[1].Value.Trim());
            }
        }

        // Default response if no patterns match
        return "Can you elaborate on that?";
    }

    public Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages,
        ChatOptions? options = null,
        CancellationToken cancellationToken = new())
    {
        string? input = messages.LastOrDefault(m => m.Role == ChatRole.User)?.Text;
        string response = Chat(input);

        ChatMessage responseMessage = new()
        {
            Role = ChatRole.Assistant,
            Contents = [new TextContent(response)]
        };
        return Task.FromResult(new ChatResponse(responseMessage));
    }

    public IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> messages,
        ChatOptions? options = null,
        CancellationToken cancellationToken = new())
    {
        return GetAsyncEnumerable();

        async IAsyncEnumerable<ChatResponseUpdate> GetAsyncEnumerable()
        {
            ChatResponse response = await GetResponseAsync(messages, options, cancellationToken);
            ChatResponseUpdate update = new(ChatRole.Assistant, response.Text);
            yield return update;
        }
    }

    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        if (serviceType == typeof(IChatClient))
        {
            return this;
        }
        return null;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
