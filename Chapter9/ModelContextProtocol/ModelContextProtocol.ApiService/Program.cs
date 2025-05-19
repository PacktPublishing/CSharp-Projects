using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.KernelMemory;
using Microsoft.KernelMemory.AI.Ollama;
using ModelContextProtocol.ApiService;
using ModelContextProtocol.ApiService.Requests;
using ModelContextProtocol.ApiService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

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

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

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

app.Run();
