using Microsoft.Extensions.AI;
using Uno.Extensions.Reactive;

namespace Chatbot.Presentation;

public partial record MainModel
{
    public MainModel()
    {
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
    }
}
