using AiPersonalAssistant.ConsoleApp.Plugins;

namespace AiPersonalAssistant.ConsoleApp.Modes;

public class KernelMemoryChatMode(IAnsiConsole console) : AlfredChatHandler(console)
{
    private IKernelMemory? _memory;

    public override async Task InitializeAsync(AlfredOptions options)
    {
        _memory = await MemoryHelpers.LoadKernelMemoryAsync(options, Console);

        await base.InitializeAsync(options);
    }

    public override async Task ChatAsync(string message)
    {
        MemoryAnswer response = await _memory!.AskAsync(message);

        Console.MarkupLine("[cyan]RAG Search Results:[/]");

        AddAssistantMessage(response.Result);
        foreach (var source in response.RelevantSources)
        {
            Console.MarkupLineInterpolated($"[grey]Used source:[/] {source.SourceName}");
        }
    }
}
