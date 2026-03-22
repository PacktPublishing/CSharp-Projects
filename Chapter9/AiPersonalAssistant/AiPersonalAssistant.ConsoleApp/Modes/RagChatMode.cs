namespace AiPersonalAssistant.ConsoleApp.Modes;

public class RagChatMode(IAnsiConsole console) : AlfredChatHandler(console)
{
    private DocumentMemory? _memory;
    private IChatClient? _chatClient;

    public override async Task InitializeAsync(AlfredOptions options)
    {
        _memory = await MemoryHelpers.LoadMemory(options, Console);
        _chatClient = new OllamaChatClient(
            new Uri(options.Endpoint), options.ChatModelId);

        await base.InitializeAsync(options);
    }

    public override async Task ChatAsync(string message)
    {
        var results = await _memory!.SearchAsync(message);

        Console.MarkupLine("[cyan]RAG Search Results:[/]");
        foreach (var (_, sourceName, _) in results)
        {
            Console.MarkupLineInterpolated($"[grey]Used source:[/] {sourceName}");
        }

        string context = string.Join("\n\n", results.Select(r =>
            $"[Source: {r.SourceName}]\n{r.Text}"));

        string augmentedPrompt = $"""
            Use the following context to answer the question. If the context doesn't contain 
            relevant information, say so.

            Context:
            {context}

            Question: {message}
            """;

        var response = await _chatClient!.GetResponseAsync(augmentedPrompt);

        AddAssistantMessage(response.Text ?? "I have no response to that.");
    }
}
