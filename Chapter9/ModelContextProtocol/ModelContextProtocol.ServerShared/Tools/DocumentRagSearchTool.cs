using System.ComponentModel;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Server;
using ModelContextProtocol.ServerShared.Requests;

namespace ModelContextProtocol.ServerShared.Tools;

[McpServerToolType]
public class DocumentRagSearchTool(IOptionsSnapshot<McpServerSettings> options)
{
    [McpServerTool(Name = "Search", Destructive = false, Idempotent = false, OpenWorld = true, ReadOnly = true)]
    [Description("Searches documents for answers to a specific question")]
    public async Task<string> Search(string question)
    {
        using HttpClient client = new();
        client.BaseAddress = new Uri(options.Value.KernelMemoryEndpoint);

        JsonContent content = JsonContent.Create(new SearchRequest
        {
            Query = question
        });

        HttpResponseMessage result = await client.PostAsync("ask", content);

        return await result.Content.ReadAsStringAsync();
    }
}
