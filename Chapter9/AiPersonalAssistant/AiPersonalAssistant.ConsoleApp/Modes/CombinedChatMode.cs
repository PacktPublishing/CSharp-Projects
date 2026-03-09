namespace AiPersonalAssistant.ConsoleApp.Modes;

public class CombinedChatMode(IAnsiConsole console) : AgentChatMode(console)
{
    private DocumentMemory? _memory;

    public override async Task InitializeAsync(AlfredOptions options)
    {
        _memory = await MemoryHelpers.LoadMemory(options, Console);

        await base.InitializeAsync(options);
    }

    public override List<AITool> GetTools()
    {
        var tools = base.GetTools();

        tools.Add(AIFunctionFactory.Create(SearchDocuments,
            name: "Search",
            description: "Searches documents and history for answers to a question"));

        return tools;
    }

    [Description("A question to search documents for")]
    private async Task<string> SearchDocuments(string question)
    {
        Console.MarkupLineInterpolated($"[cyan]RAG Search[/]: {question}");

        var results = await _memory!.SearchAsync(question);

        StringBuilder sb = new();
        foreach (var (text, sourceName, score) in results)
        {
            Console.MarkupLineInterpolated($"[grey]Source:[/] {sourceName} ({score:P2} Relevance)");
            sb.AppendLine($"Snippet found in {sourceName}:");
            sb.AppendLine(text);
        }

        return sb.ToString();
    }
}
