namespace AiPersonalAssistant.ConsoleApp;

public record AlfredOptions
{
    public required string Endpoint { get; init; }
    public required string ChatModelId { get; init; }
    public required string EmbeddingModelId { get; init; }

    public required string SystemPrompt { get; init; }
    public required string GreetingMessage { get; set; }
    public required string GoodbyeMessage { get; set; }

    public bool UseKernelMemory { get; set; }
    public bool UseSemanticKernel { get; set; }
}
