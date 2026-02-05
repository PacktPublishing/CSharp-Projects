using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;
using Spectre.Console;

IAnsiConsole console = AnsiConsole.Console;

console.Write(new FigletText("MCP Client").Color(Color.Yellow));
console.MarkupLine("[cyan]Demo client for listing MCP server contents[/]");
console.WriteLine();

string? endpoint = Environment.GetEnvironmentVariable("services__mcpserver-sse__https__0");
endpoint ??= "http://localhost:5021";
console.WriteLine($"Using endpoint for MCP Server: {endpoint}");

IClientTransport clientTransport = new SseClientTransport(new()
{
    Name = "Custom MCP Server",
    Endpoint = new Uri(endpoint),
    UseStreamableHttp = true
});

// Connect
console.WriteLine($"Connecting to MCP Server at {endpoint}...");
await using IMcpClient mcpClient = await McpClientFactory.CreateAsync(clientTransport);
Implementation info = mcpClient.ServerInfo;
console.MarkupLineInterpolated($"[green]Connected[/] to {info.Name} {info.Version}");
console.WriteLine($"Server instructions: {mcpClient.ServerInstructions ?? "No instructions set"}\r\n");

Table table = BuildMetadataTable("Prompts");
await foreach (var prompt in mcpClient.EnumeratePromptsAsync())
{
    string[] columns = { prompt.Name ?? string.Empty, prompt.Description ?? string.Empty };
    table.AddRow(columns ?? Array.Empty<string>());
}
console.Write(table);

table = BuildMetadataTable("Tools");
await foreach (var tool in mcpClient.EnumerateToolsAsync())
{
    string[] columns = { tool.Name ?? string.Empty, tool.Description ?? string.Empty };
    table.AddRow(columns ?? Array.Empty<string>());
}
console.Write(table);

table = BuildMetadataTable("Resources");
table.AddColumn("[yellow]Uri[/]");
await foreach (var res in mcpClient.EnumerateResourcesAsync())
{
    string[] columns =
    {
        res.Name ?? string.Empty,
        res.Description ?? string.Empty,
        res.Uri ?? string.Empty
    };
    table.AddRow(columns ?? Array.Empty<string>());
}
await foreach (var res in mcpClient.EnumerateResourceTemplatesAsync())
{
    string[] columns =
    {
        res.Name ?? string.Empty,
        res.Description ?? string.Empty,
        res.UriTemplate ?? string.Empty
    };
    table.AddRow(columns ?? Array.Empty<string>());
}
console.Write(table);

Table BuildMetadataTable(string title)
{
    Table table = new();
    table.Title = new TableTitle($"[Cyan]{title}[/]");

    table.AddColumn("[yellow]Name[/]");
    table.AddColumn("[yellow]Description[/]");

    table.Expand();

    return table;
}


