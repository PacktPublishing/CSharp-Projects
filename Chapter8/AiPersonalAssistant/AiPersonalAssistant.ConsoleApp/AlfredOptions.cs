using AiPersonalAssistant.ConsoleApp.Modes;

namespace AiPersonalAssistant.ConsoleApp;

public record AlfredOptions
{
    public required string Endpoint { get; init; }
    public required string ChatModelId { get; init; }
    public required string EmbeddingModelId { get; init; }

    public required string SystemPrompt { get; init; }
    public required string GreetingMessage { get; init; }
    public required string GoodbyeMessage { get; init; }

    public ApplicationMode Mode { get; init; }
    public string? DataDirectory { get; init; }
}