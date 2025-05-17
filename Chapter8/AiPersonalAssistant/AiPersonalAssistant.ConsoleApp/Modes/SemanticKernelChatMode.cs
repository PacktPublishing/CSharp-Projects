using System.Diagnostics.CodeAnalysis;
using Microsoft.SemanticKernel.Connectors.Ollama;

namespace AiPersonalAssistant.ConsoleApp.Modes;

[Experimental("SKEXP0070")]
public class SemanticKernelChatMode(IAnsiConsole console) : AlfredChatHandler(console)
{
    private Kernel? _kernel;
    private IChatCompletionService? _chat;
    private OllamaPromptExecutionSettings? _settings;
    private readonly ChatHistory _history = new();

    public override Task InitializeAsync(AlfredOptions options)
    {
        _settings = new()
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };

        _kernel = Kernel.CreateBuilder()
            .AddOllamaChatCompletion(options.ChatModelId, new Uri(options.Endpoint))
            .Build();

        // TODO: Add a bing or google search plugin if a key is specified
        // TODO: Add a simple custom skill for something silly

        _chat = _kernel.GetRequiredService<IChatCompletionService>();

        return Task.CompletedTask;
    }

    public override async Task ChatAsync(string message)
    {
        _history.AddUserMessage(message);
        IReadOnlyList<ChatMessageContent> result = await _chat!.GetChatMessageContentsAsync(_history, _settings, _kernel);

        foreach (var response in result)
        {
            AddAssistantMessage(response.Content ?? "I have no response to that.");
        }
    }

    public override void AddAssistantMessage(string message)
    {
        base.AddAssistantMessage(message);
        _history.AddAssistantMessage(message);
    }
}
