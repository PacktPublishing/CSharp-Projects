using ModelContextProtocol.Protocol;
using ModelContextProtocol.ServerShared;
using System.Diagnostics;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.Configure<McpServerSettings>(builder.Configuration.GetSection("McpServer"));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

Assembly shared = Assembly.GetAssembly(typeof(McpServerSettings))!;

// Add Model Context Protocol server implementation
builder.Services
    .AddMcpServer(o =>
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
        string version = fvi.FileVersion ?? "v0.0.1";

        o.ServerInfo = new Implementation
        {
            Name = "Custom MCP Server (SSE)",
            Version = version
        };
        o.ServerInstructions = "If no programming language is specified, assume C#. Keep your responses brief and professional.";
    })
    .WithHttpTransport()
    .WithResourcesFromAssembly(shared)
    .WithPromptsFromAssembly(shared)
    .WithToolsFromAssembly(shared);

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapMcp();

app.Run();
