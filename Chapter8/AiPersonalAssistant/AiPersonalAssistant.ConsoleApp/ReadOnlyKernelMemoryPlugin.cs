namespace AiPersonalAssistant.ConsoleApp;

public class ReadOnlyKernelMemoryPlugin(IKernelMemory memory, IAnsiConsole console)
{
    [KernelFunction("Search")]
    [Description("Searches documents and history for answers to a question")]
    public async Task<string> Search(string question)
    {
        console.MarkupLineInterpolated($"[cyan]RAG Search[/]: {question}");

        SearchResult searchResult = await memory.SearchAsync(question, limit: 5, minRelevance: 0.1);

        console.MarkupLine("[cyan]RAG Search Results:[/]");
        string json = searchResult.ToJson();
        console.Write(new JsonText(json));

        return json;
    }
}
