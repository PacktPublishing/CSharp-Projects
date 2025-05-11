using Microsoft.Extensions.AI;
using Uno.UI.RemoteControl.HotReload;

namespace AiStoryteller.Presentation;

public partial record MainModel
{
    private readonly IChatClient _chat;

    public MainModel(IChatClient chat)
    {
        _chat = chat;
        Messages.AddAsync(new ChatMessage(ChatRole.System,
            "You are an interactive chatbot designed to generate short Haikus to greet the user. Everything you send to the user must be a single Haiku related to the last message from the user. The poem must consist of three lines.\r\nThe first line must have five syllables, the second line seven syllables, and the third line five syllables. Haikus usually focus on nature or the seasons and often contain a “cutting word” that emphasizes a contrast or a change. Do not include a syllable, line, or word count or anything in your response that is not part of the Haiku."));
        Messages.AddAsync(new ChatMessage(ChatRole.Assistant, "Birds begin their song\r\nWith the melody of life, strong and long\r\nA new day has sprung."));
    }

    public IListState<ChatMessage> Messages => ListState<ChatMessage>.Empty(this);
    public IState<string> MessageText => State<string>.Value(this, () => string.Empty);
    public IState<bool> IsExecuting => State<bool>.Value(this, () => false);

    public async Task SendMessage()
    {
        string? userMessage = await MessageText.Value();
        if (string.IsNullOrWhiteSpace(userMessage))
        {
            return;
        }

        await Messages.AddAsync(new ChatMessage(ChatRole.User, userMessage));
        await MessageText.UpdateAsync(m => m = string.Empty);

        IImmutableList<ChatMessage> messages = await Messages.Value();

        await IsExecuting.UpdateAsync(m => m = true);
        ChatResponse response = await _chat.GetResponseAsync(messages);
        await IsExecuting.UpdateAsync(m => m = false);

        foreach (var message in response.Messages)
        {
            await Messages.AddAsync(message);
        }

        MainPage.ScrollToBottomHandler?.Invoke();
    }

}
