using ModelContextProtocol.ChatApi;
using ModelContextProtocol.ChatApi.Requests;
using ModelContextProtocol.ChatApi.Services;
using ModelContextProtocol.Domain.Requests;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.Configure<ChatSettings>(builder.Configuration.GetSection("Chat"));

builder.Services.AddOpenApi();
builder.Services.AddScoped<IChatService, ChatService>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
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