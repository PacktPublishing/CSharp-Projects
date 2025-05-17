using Azure;

namespace AiPersonalAssistant.ConsoleApp.Plugins;

public class ReadOnlyKernelMemoryPlugin(IKernelMemory memory, IAnsiConsole console)
{
    [KernelFunction("Search")]
    [Description("Searches documents and history for answers to a question")]
    public async Task<string> Search(string question)
    {
        console.MarkupLineInterpolated($"[cyan]RAG Search[/]: {question}");

        SearchResult searchResult = await memory.SearchAsync(question, limit: 5, minRelevance: 0.1);

        StringBuilder sb = new();
        foreach (var citation in searchResult.Results)
        {
            sb.AppendLine("Snippet found in " + citation.SourceName + ":");

            foreach (var partition in citation.Partitions)
            {
                console.MarkupLineInterpolated($"[grey]Source:[/] {citation.SourceName} ({partition.Relevance:P2} Relevance)");
                sb.AppendLine(partition.Text);
            }
        }

        return sb.ToString();
    }
}
