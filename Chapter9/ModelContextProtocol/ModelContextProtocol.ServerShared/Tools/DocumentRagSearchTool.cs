using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using ModelContextProtocol.ChatApi.Requests;
using ModelContextProtocol.Server;

namespace ModelContextProtocol.ServerShared.Tools;

[McpServerToolType]
public class DocumentRagSearchTool(IOptionsSnapshot<McpServerSettings> options)
{
    private readonly ActivitySource _activitySource = new(typeof(DocumentRagSearchTool).Assembly.FullName!);

    [McpServerTool(Name = "Search", Destructive = false, Idempotent = false, OpenWorld = true, ReadOnly = true)]
    [Description("Searches documents for answers to a specific question")]
    public async Task<string> Search(string question)
    {
        using Activity? activity = _activitySource.StartActivity(ActivityKind.Server);
        activity?.SetTag("question", question);
        
        using HttpClient client = new();
        client.BaseAddress = new Uri(options.Value.KernelMemoryEndpoint);

        JsonContent content = JsonContent.Create(new SearchRequest
        {
            Query = question
        });

        try
        {
            HttpResponseMessage result = await client.PostAsync("ask", content);

            string response = await result.Content.ReadAsStringAsync();

            activity?.AddTag("response", response);

            return response;
        }
        catch (HttpRequestException ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error);
            activity?.AddException(ex);
            throw;
        }
    }
}
