namespace AiPersonalAssistant.ConsoleApp.Modes;

public class CombinedChatMode(IAnsiConsole console) : AlfredChatHandler(console)
{
    private AIAgent? _agent;
    private AgentSession? _session;
    private DocumentMemory? _memory;

    public override async Task InitializeAsync(AlfredOptions options)
    {
        _memory = await MemoryHelpers.LoadMemory(options, Console);

        var chatClient = new OllamaChatClient(
            new Uri(options.Endpoint), options.ChatModelId);

        _agent = chatClient.AsAIAgent(
            instructions: options.SystemPrompt,
            tools: GetTools());

        _session = await _agent.CreateSessionAsync();

        await base.InitializeAsync(options);
    }

    private List<AITool> GetTools()
    {
        return
        [
            AIFunctionFactory.Create(TimeAndDateTool.GetCurrentTimeAndDate),
            AIFunctionFactory.Create(SearchDocuments,
                name: "Search",
                description: "Searches documents and history for answers to a question")
        ];
    }

    public override async Task ChatAsync(string message)
    {
        AgentResponse response = await _agent!.RunAsync(message, _session!);

        AddAssistantMessage(response.Text ?? "I have no response to that.");
    }

    private async Task<string> SearchDocuments(string question)
    {
        Console.MarkupLineInterpolated($"[cyan]RAG Search[/]: {question}");

        var results = await _memory!.SearchAsync(question);

        if (!results.Any())
        {
            Console.MarkupLine("[grey]No relevant documents found.[/]");
            return "No relevant documents found.";
        }

        StringBuilder sb = new();
        foreach (SearchResult result in results)
        {
            Console.MarkupLineInterpolated($"[grey]Source:[/] {result.SourceName} ({result.Score:P2} Relevance)");
            sb.AppendLine($"Snippet found in {result.SourceName}:");
            sb.AppendLine(result.Text);
        }

        return sb.ToString();
    }
}
