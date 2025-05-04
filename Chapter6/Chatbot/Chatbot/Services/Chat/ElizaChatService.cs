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
    private static string Chat(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return "Please tell me more.";
        }

        const RegexOptions regexOptions = RegexOptions.Compiled | RegexOptions.IgnoreCase;
        (Regex Pattern, string Response)[] patterns =
        [
            (new Regex(@"\bI need (.*)", regexOptions), "Why do you need {0}?"),
            (new Regex(@"\bI am (.*)", regexOptions), "How long have you been {0}?"),
            (new Regex(@"\bI feel (.*)", regexOptions), "Why do you feel {0}?"),
            (new Regex(@"\bBecause (.*)", regexOptions), "Is that the real reason?"),
            (new Regex(@"\bWhy don't you (.*)", regexOptions), "What makes you think I don't {0}?"),
            (new Regex(@"\bWhy can't I (.*)", regexOptions), "What makes you think you can't {0}?"),
            (new Regex(@"\bI can't (.*)", regexOptions), "What would it take for you to {0}?"),
            (new Regex(@"\bI think (.*)", regexOptions), "Why do you think {0}?"),
            (new Regex(@"\bI want (.*)", regexOptions), "What would it mean if you got {0}?"),
            (new Regex(@"\b(.*) mother(.*)", regexOptions), "Tell me more about your mother."),
            (new Regex(@"\b(.*) father(.*)", regexOptions), "Tell me more about your father."),
            (new Regex(@"\b(.*)\?", regexOptions), "What do you think?")
        ];

        foreach (var (pattern, response) in patterns)
        {
            Match match = pattern.Match(input);
            if (match.Success)
            {
                return string.Format(response, match.Groups[1].Value.Trim());
            }
        }

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
