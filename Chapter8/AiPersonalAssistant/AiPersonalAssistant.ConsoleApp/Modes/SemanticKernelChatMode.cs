namespace AiPersonalAssistant.ConsoleApp.Modes;

public class SemanticKernelChatMode(IAnsiConsole console) : AlfredChatHandler(console)
{
    private Kernel? _kernel;
    private IChatCompletionService? _chat;
    private readonly ChatHistory _history = new();
    protected Kernel? Kernel => _kernel;


    [Experimental("SKEXP0070")]
    public override Task InitializeAsync(AlfredOptions options)
    {
        _kernel = Kernel.CreateBuilder()
            .AddOllamaChatCompletion(options.ChatModelId, new Uri(options.Endpoint))
            .Build();
        
        _kernel.ImportPluginFromType<TimeAndDatePlugin>();

        _chat = _kernel.GetRequiredService<IChatCompletionService>();

        return Task.CompletedTask;
    }

    [Experimental("SKEXP0070")]
    public override async Task ChatAsync(string message)
    {
        _history.AddUserMessage(message);

        FunctionChoiceBehaviorOptions options = new()
        {
            AllowConcurrentInvocation = true,
            AllowParallelCalls = true,
        };
        OllamaPromptExecutionSettings settings = new()
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(options: options),
        };

        IReadOnlyList<ChatMessageContent> result = await _chat!.GetChatMessageContentsAsync(_history, settings, _kernel);

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
