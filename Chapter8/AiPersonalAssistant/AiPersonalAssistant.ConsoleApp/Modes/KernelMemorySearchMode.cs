using DocumentFormat.OpenXml.Presentation;

namespace AiPersonalAssistant.ConsoleApp.Modes;

public class KernelMemorySearchMode(IAnsiConsole console) : AlfredChatHandler(console)
{
    private IKernelMemory? _memory;

    public override async Task InitializeAsync(AlfredOptions options)
    {
        _memory = await MemoryHelpers.LoadMemory(options, Console);

        await base.InitializeAsync(options);
    }

    public override async Task ChatAsync(string message)
    {
        SearchResult response = await _memory!.SearchAsync(message);

        foreach (var cite in response.Results)
        {
            foreach (var part in cite.Partitions)
            {
                const int snippetLength = 50;
                string match = part.Text.ReplaceLineEndings("").Trim();
                match = match.Length > snippetLength
                    ? $"{match[..snippetLength]}..."
                    : match;

                AddAssistantMessage($"{cite.SourceName} ({part.Relevance:P2} Relevance): {match}");
            }
        }
    }
}
