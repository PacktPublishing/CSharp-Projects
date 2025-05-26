namespace AiPersonalAssistant.ConsoleApp.Modes;

public class SemanticKernelChatMode(IAnsiConsole console) : AlfredChatHandler(console)
{
    private Kernel? _kernel;
    private IChatCompletionService? _chat;
    private readonly ChatHistory _history = new();

    [Experimental("SKEXP0070")]
    public override async Task InitializeAsync(AlfredOptions options)
    {
        _kernel = Kernel.CreateBuilder()
            .AddOllamaChatCompletion(options.ChatModelId, new Uri(options.Endpoint))
            .Build();

        LoadPlugins(_kernel);

        _chat = _kernel.GetRequiredService<IChatCompletionService>();

        await base.InitializeAsync(options);
    }

    public virtual void LoadPlugins(Kernel kernel)
    {
        kernel.ImportPluginFromType<TimeAndDatePlugin>();
    }

    [Experimental("SKEXP0070")]
    public override async Task ChatAsync(string message)
    {
        _history.AddUserMessage(message);

        FunctionChoiceBehaviorOptions behave = new()
        {
            AllowConcurrentInvocation = true,
        };
        OllamaPromptExecutionSettings settings = new()
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(options: behave)
        };

        IEnumerable<ChatMessageContent> result = await _chat!.GetChatMessageContentsAsync(_history, settings, _kernel);

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
