using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#pragma warning disable SKEXP0070
IAnsiConsole console = AnsiConsole.Console;

try
{
    AlfredOptions options = new(); // TODO: Read from config file and environment variables

    Console.OutputEncoding = Encoding.Unicode;
    Console.InputEncoding = Encoding.Unicode;

    DisplayAppHeader(console);

    // Build the kernel
    Uri ollamaEndpoint = new Uri(options.Endpoint);
    IKernelBuilder builder = Kernel.CreateBuilder();
    builder.Services.AddLogging(c =>
    {
        c.AddSimpleConsole();
        c.SetMinimumLevel(LogLevel.Trace);
    }); 

    Kernel kernel = builder
        .AddOllamaChatCompletion(options.ChatModelId, ollamaEndpoint)
        .AddOllamaTextEmbeddingGeneration(options.EmbeddingModelId, ollamaEndpoint)
        .AddInMemoryVectorStore()
        .Build();

    OllamaConfig config = new()
    {
        Endpoint = options.Endpoint,
        TextModel = new OllamaModelConfig(options.ChatModelId),
        EmbeddingModel = new OllamaModelConfig(options.EmbeddingModelId) // 2048
    };

    MemoryServerless mem = new KernelMemoryBuilder()
        .WithOllamaTextGeneration(config)
        .WithOllamaTextEmbeddingGeneration(config)
        .Build<MemoryServerless>();

    await mem.ImportTextAsync(File.ReadAllText("Facts.txt"));
    kernel.ImportPluginFromObject(new MemoryPlugin(mem));

    // Initialize chat history
    IChatCompletionService chatService = kernel.GetRequiredService<IChatCompletionService>();
    ChatHistory history = new(options.SystemPrompt);
    AddAssistantMessage(history, "Hello. How may I help you today? Up to any coding sprees?");

    HashSet<string> exitWords = new(StringComparer.OrdinalIgnoreCase) {
        "exit", "quit", "q", "e", "x", "bye", "goodbye"
    };

    PromptExecutionSettings settings = new()
    {
        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
    };

    // Main conversation loop
    string? reply;
    do
    {
        reply = console.Prompt(new TextPrompt<string>("[orange3]User[/]: "));
        history.AddUserMessage(reply);

        IReadOnlyList<ChatMessageContent> response = [];
        await console.Status().StartAsync("Thinking...",
            async _ =>
            {
                response = await chatService.GetChatMessageContentsAsync(history, settings, kernel);
            });

        foreach (var part in response)
        {
            AddAssistantMessage(history, part.Content ?? "I have no response to that");
        }
    } while (!string.IsNullOrWhiteSpace(reply) && !exitWords.Contains(reply));

    AddAssistantMessage(history, "Goodbye, and happy coding.");

    return 0;
}
catch (Exception ex)
{
    console.WriteException(ex, ExceptionFormats.ShortenEverything);
    return -1;
}

void DisplayAppHeader(IAnsiConsole ansiConsole)
{
    Style accentStyle = new Style(Color.Aqua);

    ansiConsole.Write(new FigletText("Alfred").Color(Color.Yellow));
    ansiConsole.WriteLine("Digital butler to the budding super-coder", accentStyle);
    ansiConsole.WriteLine();
}

void AddAssistantMessage(ChatHistory chatHistory, string message)
{
    chatHistory.AddAssistantMessage(message);
    console.MarkupLineInterpolated($"[yellow]Alfred[/]: {message}");
}

#pragma warning restore SKEXP0070
