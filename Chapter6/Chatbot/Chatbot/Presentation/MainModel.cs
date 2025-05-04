using Microsoft.Extensions.AI;
using Uno.Extensions.Reactive;

namespace Chatbot.Presentation;

public partial record MainModel
{
    private INavigator _navigator;

    public MainModel(
        IStringLocalizer localizer,
        IOptions<AppConfig> appInfo,
        INavigator navigator)
    {
        _navigator = navigator;

        Messages.AddAsync(new ChatMessage(ChatRole.Assistant, "Hello, I'm ELIZA. What's going on right now?"));
    }

    public IListState<ChatMessage> Messages => ListState<ChatMessage>.Empty(this);

    public IState<string> Name => State<string>.Value(this, () => string.Empty);

    public async Task GoToSecond()
    {
        var name = await Name;
        await _navigator.NavigateViewModelAsync<SecondModel>(this, data: new Entity(name!));
    }

    public async Task SendMessage()
    {
        await Task.CompletedTask;
    }
}
