using Microsoft.Extensions.Options;
using Microsoft.KernelMemory;
using Microsoft.KernelMemory.AI.Ollama;
using ModelContextProtocol.ApiService;
using ModelContextProtocol.ApiService.Services;
using ModelContextProtocol.DocumentsApi.Requests;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.Configure<OllamaSearchOptions>(
    builder.Configuration.GetSection("OllamaOptions"));

builder.Services.AddSingleton<IKernelMemory>(m =>
{
    IOptions<OllamaSearchOptions> snapshot = m.GetRequiredService<IOptions<OllamaSearchOptions>>();
    OllamaSearchOptions options = snapshot.Value;

    OllamaConfig config = new()
    {
        Endpoint = options.Endpoint,
        TextModel = new OllamaModelConfig(options.ChatModelId),
        EmbeddingModel = new OllamaModelConfig(options.EmbeddingModelId)
    };

    return new KernelMemoryBuilder()
        .WithOllamaTextGeneration(config)
        .WithOllamaTextEmbeddingGeneration(config)
        .Build<MemoryServerless>();
});
builder.Services.AddHostedService<DocumentIndexingService>();
builder.Services.AddScoped<ISearchService, SearchService>();


var app = builder.Build();

// Map endpoints
app.MapPost("/search", async (SearchRequest req, ISearchService search) =>
{
    string response = await search.Search(req.Query);
    return Results.Ok(response);
});
app.MapPost("/ask", async (SearchRequest req, ISearchService search) =>
{
    string response = await search.Ask(req.Query);
    return Results.Ok(response);
});
app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
