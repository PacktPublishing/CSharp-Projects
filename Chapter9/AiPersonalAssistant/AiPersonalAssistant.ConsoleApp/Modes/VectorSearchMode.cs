namespace AiPersonalAssistant.ConsoleApp.Modes;

public class VectorSearchMode(IAnsiConsole console) : AlfredChatHandler(console)
{
    private DocumentMemory? _memory;

    public override async Task InitializeAsync(AlfredOptions options)
    {
        _memory = await MemoryHelpers.LoadMemory(options, Console);
        await base.InitializeAsync(options);
    }

    public override async Task ChatAsync(string message)
    {
        var results = await _memory!.SearchAsync(message);

        foreach (var (text, sourceName, score) in results)
        {
            const int snippetLength = 50;
            string match = text.ReplaceLineEndings("").Trim();
            match = match.Length > snippetLength
                ? $"{match[..snippetLength]}..."
                : match;

            AddAssistantMessage($"{sourceName} ({score:P2} Relevance): {match}");
        }
    }
}
