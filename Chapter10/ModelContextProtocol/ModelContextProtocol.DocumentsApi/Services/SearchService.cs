using System.Text;
using Microsoft.KernelMemory;

namespace ModelContextProtocol.DocumentsApi.Services;

public class SearchService(IKernelMemory memory) : ISearchService
{
    public async Task<string> Search(string query)
    {
        SearchResult searchResult = await memory.SearchAsync(query, limit: 5);

        if (searchResult.NoResult) return "There were no relevant search results";

        StringBuilder sb = new();
        foreach (var cite in searchResult.Results)
        {
            sb.AppendLine($"Snippet found in {cite.SourceName}:");

            foreach (var part in cite.Partitions)
            {
                sb.AppendLine(part.Text);
            }
        }

        return sb.ToString();
    }

    public async Task<string> Ask(string query)
    {
        MemoryAnswer response = await memory.AskAsync(query);

        return response.Result;
    }
}