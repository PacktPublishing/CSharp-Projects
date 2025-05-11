using AiStoryteller.StoryServices;
using Microsoft.Extensions.AI;

namespace AiStoryteller.Presentation;

public partial record MainModel
{
    private readonly IChatPartner _partner;

    public MainModel(IChatPartner partner)
    {
        _partner = partner;

        foreach (var message in _partner.Messages.Where(m => m.Role != ChatRole.System))
        {
            Messages.AddAsync(message);
        }
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

        ChatMessage chatMessage = new(ChatRole.User, userMessage);
        await Messages.AddAsync(chatMessage);
        await MessageText.UpdateAsync(m => m = string.Empty);

        await IsExecuting.UpdateAsync(m => m = true);
        string response = await _partner.Chat(userMessage);

        await Messages.AddAsync(new ChatMessage(ChatRole.Assistant, response));

        await IsExecuting.UpdateAsync(m => m = false);
        MainPage.ScrollToBottomHandler?.Invoke();
    }

}
