using Microsoft.Extensions.AI;

namespace AiStoryteller.StoryServices;

public class HaikuChatPartner(IChatClient chatClient) : IChatPartner
{
    private readonly List<ChatMessage> _messages =
    [
        new(ChatRole.System,
            "You are an interactive chatbot designed to generate short Haikus. " +
            "Everything you send to the user must be a Haiku." +
            "The Haiku must consist of three lines." +
            "The first line must have five syllables, the second line seven syllables, and the third line five syllables." +
            "Haikus usually focus on nature or the seasons and often contain a “cutting word” that emphasizes a contrast or a change." +
            "Only respond with the final Haiku. Do not include any syllable, line, or word count or reasoning in your response."),


        new(ChatRole.Assistant, """
            Birds begin their song
            With the melody of life, strong and long
            A new day has sprung.
            """)
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
