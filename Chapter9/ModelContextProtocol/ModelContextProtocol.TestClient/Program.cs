using Microsoft.Extensions.Hosting;
using ModelContextProtocol.Client;

//var builder = Host.CreateApplicationBuilder(args);
//builder.AddServiceDefaults();

StdioClientTransport clientTransport = new(new()
{
    Name = "Custom MCP Server",
    Command = "dotnet",
    Arguments = ["run", "--project", "../../../../ModelContextProtocol.CustomServer"]//, "--no-build"],
});

Console.WriteLine("Connecting to MCP Server");
await using IMcpClient mcpClient = await McpClientFactory.CreateAsync(clientTransport);
Console.WriteLine("MCP Server started");

await mcpClient.PingAsync();
Console.WriteLine("Connection verified");

Console.WriteLine("\r\nListing Prompts");
await foreach (var prompt in mcpClient.EnumeratePromptsAsync())
{
    Console.WriteLine($"{prompt.Name}: {prompt.Description}");
}

Console.WriteLine("\r\nListing Tools");
await foreach (var tool in mcpClient.EnumerateToolsAsync())
{
    Console.WriteLine($"{tool.Name}: {tool.Description}");
}

Console.WriteLine("\r\nListing Resources");
await foreach (var res in mcpClient.EnumerateResourcesAsync())
{
    Console.WriteLine($"{res.Name}: {res.Description}");
}

/*
var tools = await mcpClient.ListToolsAsync();
foreach (var tool in tools)
{
    Console.WriteLine($"Connected to server with tools: {tool.Name}");
}
*/

//var app = builder.Build();
//app.Run();

