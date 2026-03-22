using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using ModelContextProtocol.ChatApi;
using ModelContextProtocol.ChatApi.Services;
using ModelContextProtocol.Client;
using ModelContextProtocol.Domain.Requests;
using ModelContextProtocol.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.Configure<ChatSettings>(builder.Configuration.GetSection("Chat"));

builder.Services.AddOpenApi();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddSingleton<AIAgent>(sp =>
{
    ChatSettings options = sp.GetRequiredService<IOptions<ChatSettings>>().Value;
    ILoggerFactory? loggerFactory = sp.GetRequiredService<ILoggerFactory>();

    Uri endpoint = new(options.ChatEndpoint);
    string modelId = options.ChatModelId;
    OllamaChatClient innerClient = new(endpoint, modelId);

    var chatClient = new ChatClientBuilder(innerClient)
        .UseOpenTelemetry(loggerFactory, configure: c => c.EnableSensitiveData = true)
        .Build();

    var tools = sp.GetRequiredService<IList<AITool>>();
    return chatClient.AsAIAgent(
        instructions: options.SystemPrompt,
        tools: tools,
        loggerFactory: loggerFactory);
});

builder.Services.AddSingleton<IList<AITool>>(sp =>
{
    IMcpClient client = sp.GetRequiredService<IMcpClient>();
    IList<McpClientTool> tools = client.ListToolsAsync().Result;
    return tools.Cast<AITool>().ToList();
});

builder.Services.AddSingleton<IMcpClient>(sp =>
{
    IClientTransport clientTransport = sp.GetRequiredService<IClientTransport>();
    return McpClientFactory.CreateAsync(clientTransport).GetAwaiter().GetResult();
});
builder.Services.AddSingleton<IClientTransport>(sp =>
{
    var options = sp.GetRequiredService<IOptions<ChatSettings>>().Value;
    return new SseClientTransport(new SseClientTransportOptions
    {
        Name = "Custom MCP Server",
        Endpoint = new Uri(options.McpServerEndpoint),
        UseStreamableHttp = true
    });
});

WebApplication app = builder.Build();
app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("chat", async (ChatRequest request, IChatService chat) =>
{
    var result = chat.ChatAsync(request);

    List<string> replies = [];
    await foreach (var reply in result)
    {
        replies.Add(reply);
    }

    return Results.Ok(replies);
});

app.Run();