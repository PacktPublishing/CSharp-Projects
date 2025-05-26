using Azure;

namespace AiPersonalAssistant.ConsoleApp.Plugins;

public class ReadOnlyKernelMemoryPlugin(IKernelMemory memory, IAnsiConsole console)
{
    [KernelFunction("Search")]
    [Description("Searches documents and history for answers to a question")]
    public async Task<string> Search([Description("A question to search documents for")] string question)
    {
        console.MarkupLineInterpolated($"[cyan]RAG Search[/]: {question}");

        SearchResult searchResult = await memory.SearchAsync(question, limit: 5, minRelevance: 0.1);

        StringBuilder sb = new();
        foreach (var cite in searchResult.Results)
        {
            sb.AppendLine($"Snippet found in {cite.SourceName}:");

            foreach (var part in cite.Partitions)
            {
                console.MarkupLineInterpolated($"[grey]Source:[/] {cite.SourceName} ({part.Relevance:P2} Relevance)");
                sb.AppendLine(part.Text);
            }
        }

        return sb.ToString();
    }
}
