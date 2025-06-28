#pragma warning disable SKEXP0070
#pragma warning disable SKEXP0001

using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
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
builder.Services.AddSingleton<IKernelBuilder>(sp =>
{
    var options = sp.GetRequiredService<IOptions<ChatSettings>>().Value;
    IKernelBuilder kernelBuilder = Kernel.CreateBuilder()
        .AddOllamaChatCompletion(options.ChatModelId, new Uri(options.ChatEndpoint));
    
    ILoggerFactory logFactory = sp.GetRequiredService<ILoggerFactory>();
    kernelBuilder.Services.AddSingleton(logFactory);

    return kernelBuilder;
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
builder.Services.AddSingleton<Kernel>(sp =>
{
    IKernelBuilder kernelBuilder = sp.GetRequiredService<IKernelBuilder>();
    Kernel kernel = kernelBuilder.Build();
    
    IMcpClient client = sp.GetRequiredService<IMcpClient>();
    IList<McpClientTool> tools = client.ListToolsAsync().Result;
    foreach (var tool in tools)
    {
        kernel.Plugins.AddFromFunctions(tool.Name, tool.Description, [tool.AsKernelFunction()]);
    }
    
    return kernel;
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