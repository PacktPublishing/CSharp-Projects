using Microsoft.Extensions.Configuration;

#pragma warning disable SKEXP0070
IAnsiConsole console = AnsiConsole.Console;

try
{
    Console.OutputEncoding = Encoding.Unicode;
    Console.InputEncoding = Encoding.Unicode;

    IConfiguration config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
        .AddEnvironmentVariables()
        .AddUserSecrets<Program>()
        .AddCommandLine(args)
        .Build();

    AlfredOptions options = config.Get<AlfredOptions>()!;

    DisplayAppHeader(console);

    // Build the kernel
    Uri ollamaEndpoint = new Uri(options.Endpoint);
    Kernel kernel = Kernel.CreateBuilder()
        .AddOllamaChatCompletion(options.ChatModelId, ollamaEndpoint)
        .AddOllamaTextEmbeddingGeneration(options.EmbeddingModelId, ollamaEndpoint)
        .AddInMemoryVectorStore()
        .Build();

    MemoryPlugin memory = await MemoryHelpers.CreateKernelMemoryAsync(options, console);
    kernel.ImportPluginFromObject(memory);

    // Initialize chat history
    IChatCompletionService chatService = kernel.GetRequiredService<IChatCompletionService>();
    ChatHistory history = new(options.SystemPrompt);
    AddAssistantMessage(history, options.GreetingMessage);

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

        if (exitWords.Contains(reply)) break;

        IReadOnlyList<ChatMessageContent> response = [];
        await console.Status().StartAsync("Thinking...",
            async _ => { response = await chatService.GetChatMessageContentsAsync(history, settings, kernel); });

        foreach (var part in response)
        {
            AddAssistantMessage(history, part.Content ?? "I have no response to that");
        }
    } while (!string.IsNullOrWhiteSpace(reply));

    AddAssistantMessage(history, options.GoodbyeMessage);

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
