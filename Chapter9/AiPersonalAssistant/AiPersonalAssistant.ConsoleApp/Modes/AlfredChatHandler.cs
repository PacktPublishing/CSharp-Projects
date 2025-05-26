namespace AiPersonalAssistant.ConsoleApp.Modes;

public abstract class AlfredChatHandler(IAnsiConsole console)
{
    public IAnsiConsole Console { get; } = console;

    public virtual Task InitializeAsync(AlfredOptions options)
    {
        AddAssistantMessage(options.GreetingMessage);

        return Task.CompletedTask;
    }

    public abstract Task ChatAsync(string message);

    public virtual void AddAssistantMessage(string message)
    {
        Console.MarkupLineInterpolated($"[yellow]Alfred[/]: {message}");
    }
}