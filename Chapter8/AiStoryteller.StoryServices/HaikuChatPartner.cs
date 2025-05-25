using System.ComponentModel;
using Microsoft.Extensions.AI;

namespace AiStoryteller.StoryServices;

public class HaikuChatPartner(IChatClient chatClient) : IChatPartner
{
    private readonly List<ChatMessage> _messages =
    [
        new(ChatRole.System,
            "You are an interactive chatbot designed to generate short Haikus. " +
            "Everything you send to the user must be a Haiku and should reference the current time of day if possible." +
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

    private readonly ChatOptions _options = new()
    {
        Tools = [AIFunctionFactory.Create(() =>
        {
            DateTime time = DateTime.Now;
            string timeOfDay = time.ToString("hh:mm tt");

            return time.Hour switch
            {
                < 12 => $"The time is {timeOfDay} in the morning.",
                < 17 => $"The time is {timeOfDay} in the afternoon.",
                _ => $"The time is {timeOfDay} in the evening."
            };
        }, "TimeOfDay", "Gets a description of the current time of day")],
        ToolMode = ChatToolMode.Auto,
        AllowMultipleToolCalls = false,
    };

    public IEnumerable<ChatMessage> Messages => _messages.AsReadOnly();

    public async Task<string> Chat(string message)
    {
        _messages.Add(new ChatMessage(ChatRole.User, message));

        ChatResponse response;
        try
        {
            response = await chatClient.GetResponseAsync(_messages, _options);
        }
        catch (InvalidOperationException ex)
        {
            return "An error occurred: " + ex.Message;
        }

        _messages.AddRange(response.Messages);

        return response.Text;
    }
}
