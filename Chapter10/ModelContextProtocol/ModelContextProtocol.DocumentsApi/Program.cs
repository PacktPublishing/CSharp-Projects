using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using ModelContextProtocol.DocumentsApi;
using ModelContextProtocol.DocumentsApi.Requests;
using ModelContextProtocol.DocumentsApi.Services;
using ModelContextProtocol.ServiceDefaults;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddOpenApi();

builder.Services.Configure<OllamaSearchOptions>(
    builder.Configuration.GetSection("OllamaOptions"));

builder.Services.AddSingleton<IEmbeddingGenerator<string, Embedding<float>>>(sp =>
{
    IOptions<OllamaSearchOptions> snapshot = sp.GetRequiredService<IOptions<OllamaSearchOptions>>();
    OllamaSearchOptions options = snapshot.Value;
    return new OllamaEmbeddingGenerator(new Uri(options.Endpoint), options.EmbeddingModelId);
});
builder.Services.AddSingleton<IChatClient>(sp =>
{
    IOptions<OllamaSearchOptions> snapshot = sp.GetRequiredService<IOptions<OllamaSearchOptions>>();
    OllamaSearchOptions options = snapshot.Value;
    return new OllamaChatClient(new Uri(options.Endpoint), options.ChatModelId);
});
builder.Services.AddSingleton<List<DocumentChunk>>();
builder.Services.AddSingleton<DocumentMemory>();
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
