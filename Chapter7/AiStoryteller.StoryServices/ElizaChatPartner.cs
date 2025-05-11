using System.Text.RegularExpressions;
using Microsoft.Extensions.AI;

namespace AiStoryteller.StoryServices;

public class ElizaChatPartner : IChatPartner
{
    public Task<string> Chat(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return Task.FromResult("Please tell me more.");
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
                return Task.FromResult(string.Format(response, match.Groups[1].Value.Trim()));
            }
        }

        return Task.FromResult("Can you elaborate on that?");
    }

    public IEnumerable<ChatMessage> Messages =>
    [
        new(ChatRole.Assistant, "Hello, I'm ELIZA. What's going on?")
    ];
}
