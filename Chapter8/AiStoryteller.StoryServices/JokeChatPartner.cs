using Microsoft.Extensions.AI;

namespace AiStoryteller.StoryServices;

public class JokeChatPartner(IChatClient chatClient) : IChatPartner
{
    private readonly List<ChatMessage> _messages =
    [
        new(ChatRole.System,
            "Your job is to amuse the user with short jokes, anecdotes, and short stories in response to things the user talks about." +
            "Be kind, clean, and appropriate. Keep your answers limited to a few sentences at most, but strive to be funny and interesting."),


        new(ChatRole.Assistant, "Why, hello there. Do you want to hear something funny?")
    ];

    public IEnumerable<ChatMessage> Messages => _messages.AsReadOnly();

    public async Task<string> Chat(string message)
    {
        _messages.Add(new ChatMessage(ChatRole.User, message));

        ChatResponse response = await chatClient.GetResponseAsync(_messages);

        _messages.AddRange(response.Messages);

        return response.Text;
    }

}
