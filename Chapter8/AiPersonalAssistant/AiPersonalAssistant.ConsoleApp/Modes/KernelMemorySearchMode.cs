using AiPersonalAssistant.ConsoleApp.Plugins;

namespace AiPersonalAssistant.ConsoleApp.Modes;

public class KernelMemorySearchMode(IAnsiConsole console) : AlfredChatHandler(console)
{
    private IKernelMemory? _memory;

    public override async Task InitializeAsync(AlfredOptions options)
    {
        _memory = await MemoryHelpers.LoadKernelMemoryAsync(options, Console);
    }

    public override async Task ChatAsync(string message)
    {
        SearchResult response = await _memory!.SearchAsync(message);

        Console.MarkupLine("[cyan]RAG Search Results:[/]");

        string json = response.ToJson();
        Console.Write(new JsonText(json));
        Console.WriteLine();

        foreach (var citation in response.Results)
        {
            foreach (var partition in citation.Partitions)
            {
                AddAssistantMessage($"{citation.SourceName} ({partition.Relevance:P2} Relevance)");
            }
        }
    }
}
