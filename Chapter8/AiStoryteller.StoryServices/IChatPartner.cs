using Microsoft.Extensions.AI;

namespace AiStoryteller.StoryServices;

public interface IChatPartner
{
    Task<string> Chat(string message);

    IEnumerable<ChatMessage> Messages { get; }
}