using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Logging.AddConsole(options =>
{
    options.LogToStandardErrorThreshold = LogLevel.Trace;
});

builder.Services
    .AddMcpServer(o =>
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
        string version = fvi.FileVersion ?? "v0.0.1";

        o.ServerInfo = new Implementation
        {
            Name = "Custom MCP Server",
            Version = version
        };
        o.ServerInstructions = "If no programming language is specified, assume C#. Keep your responses brief and professional.";
    })
    .WithStdioServerTransport()
    .WithResourcesFromAssembly()
    .WithPromptsFromAssembly()
    .WithToolsFromAssembly();

await builder.Build().RunAsync();
