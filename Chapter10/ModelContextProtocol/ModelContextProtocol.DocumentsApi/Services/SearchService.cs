using System.Text;
using Microsoft.Extensions.AI;

namespace ModelContextProtocol.DocumentsApi.Services;

public class SearchService(DocumentMemory memory, IChatClient chatClient) : ISearchService
{
    public async Task<string> Search(string query)
    {
        var results = await memory.SearchAsync(query, limit: 5);

        if (results.Count == 0) return "There were no relevant search results";

        StringBuilder sb = new();
        foreach (var (text, sourceName, score) in results)
        {
            sb.AppendLine($"Snippet found in {sourceName} (relevance: {score:F2}):");
            sb.AppendLine(text);
        }

        return sb.ToString();
    }

    public async Task<string> Ask(string query)
    {
        var results = await memory.SearchAsync(query, limit: 5);

        if (results.Count == 0) return "I could not find any relevant information to answer that question.";

        StringBuilder context = new();
        foreach (var (text, sourceName, _) in results)
        {
            context.AppendLine($"From {sourceName}:");
            context.AppendLine(text);
            context.AppendLine();
        }

        string augmentedPrompt = $"""
            Use the following context to answer the question. If the context does not contain relevant information, say so.

            Context:
            {context}

            Question: {query}
            """;

        ChatResponse response = await chatClient.GetResponseAsync(augmentedPrompt);
        return response.Text ?? "No response generated.";
    }
}