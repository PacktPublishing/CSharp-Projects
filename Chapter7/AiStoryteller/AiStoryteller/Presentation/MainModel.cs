using Microsoft.Extensions.AI;

namespace AiStoryteller.Presentation;

public partial record MainModel
{
    private readonly IChatClient _chat;

    public MainModel(IChatClient chat)
    {
        _chat = chat;
        Messages.AddAsync(new ChatMessage(ChatRole.Assistant, "Hello, I'm ELIZA. What's going on right now?"));
    }

    public IListState<ChatMessage> Messages => ListState<ChatMessage>.Empty(this);
    public IState<string> MessageText => State<string>.Value(this, () => string.Empty);

    public async Task SendMessage()
    {
        string? userMessage = await MessageText.Value();
        if (string.IsNullOrWhiteSpace(userMessage))
        {
            return;
        }

        await Messages.AddAsync(new ChatMessage(ChatRole.User, userMessage));
        await MessageText.UpdateAsync(m => m = string.Empty);

        ChatResponse response = await _chat.GetResponseAsync(userMessage);
        foreach (var message in response.Messages)
        {
            await Messages.AddAsync(message);
        }

        MainPage.ScrollToBottomHandler?.Invoke();
    }
}
