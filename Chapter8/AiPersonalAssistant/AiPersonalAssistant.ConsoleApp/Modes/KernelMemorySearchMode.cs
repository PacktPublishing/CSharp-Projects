using DocumentFormat.OpenXml.Presentation;

namespace AiPersonalAssistant.ConsoleApp.Modes;

public class KernelMemorySearchMode(IAnsiConsole console) : AlfredChatHandler(console)
{
    private IKernelMemory? _memory;

    public override async Task InitializeAsync(AlfredOptions options)
    {
        _memory = await MemoryHelpers.LoadKernelMemoryAsync(options, Console);

        await base.InitializeAsync(options);
    }

    public override async Task ChatAsync(string message)
    {
        SearchResult response = await _memory!.SearchAsync(message);

        foreach (var citation in response.Results)
        {
            foreach (var partition in citation.Partitions)
            {
                const int snippetLength = 50;
                string match = partition.Text.ReplaceLineEndings("").Trim();
                match = match.Length > snippetLength
                    ? $"{match[..snippetLength]}..."
                    : match;

                AddAssistantMessage($"{citation.SourceName} ({partition.Relevance:P2} Relevance): {match}");
            }
        }
    }
}
