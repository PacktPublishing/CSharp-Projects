namespace AiPersonalAssistant.ConsoleApp.Modes;

public class KernelMemoryChatMode(IAnsiConsole console) : AlfredChatHandler(console)
{
    private IKernelMemory? _memory;

    public override async Task InitializeAsync(AlfredOptions options)
    {
        _memory = await MemoryHelpers.LoadKernelMemoryAsync(options, Console);
    }

    public override async IAsyncEnumerable<string> ChatAsync(string message)
    {
        MemoryAnswer response = await _memory!.AskAsync(message);

        Console.MarkupLine("[cyan]RAG Search Results:[/]");

        string json = response.ToJson(optimizeForStream: false);
        Console.Write(new JsonText(json));
        Console.WriteLine();

        yield return response.Result;
    }
}
