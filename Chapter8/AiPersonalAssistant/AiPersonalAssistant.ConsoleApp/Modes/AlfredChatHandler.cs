namespace AiPersonalAssistant.ConsoleApp.Modes;

public abstract class AlfredChatHandler(IAnsiConsole console)
{
    public IAnsiConsole Console { get; } = console;

    public virtual Task InitializeAsync(AlfredOptions options)
    {
        return Task.CompletedTask;
    }

    public abstract IAsyncEnumerable<string> ChatAsync(string message);

    public virtual void AddAssistantMessage(string message)
    {
        Console.MarkupLineInterpolated($"[yellow]Alfred[/]: {message}");

        // TODO: Store the interaction in a chat history file for next session
    }

    public virtual string GetUserMessage()
    {
        TextPrompt<string> prompt = new("[orange3]User[/]: ");
        string message = console.Prompt(prompt);

        // TODO: Store the interaction

        return message;
    }
}