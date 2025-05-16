using Microsoft.Extensions.Configuration;

#pragma warning disable SKEXP0070

// Set up the console
IAnsiConsole console = AnsiConsole.Console;
Console.OutputEncoding = Encoding.Unicode;
Console.InputEncoding = Encoding.Unicode;
DisplayAppHeader(console);

try
{
    // Load options
    IConfiguration config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
        .AddEnvironmentVariables()
        .AddUserSecrets<Program>()
        .AddCommandLine(args)
        .Build();
    AlfredOptions options = config.Get<AlfredOptions>()!;

    IKernelMemory memory = await MemoryHelpers.LoadKernelMemoryAsync(options, console);

    // Build the kernel
    Kernel kernel = Kernel.CreateBuilder()
        .AddOllamaChatCompletion(options.ChatModelId, new Uri(options.Endpoint))
        .Build();

    kernel.ImportPluginFromObject(new ReadOnlyKernelMemoryPlugin(memory, console));

    // TODO: Add a bing or google search plugin if a key is specified

    // TODO: Add a simple custom skill for something silly

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

        // TODO: Adding logging at a function call level would be helpful

        IReadOnlyList<ChatMessageContent> response = await chatService.GetChatMessageContentsAsync(history, settings, kernel);

        foreach (var part in response)
        {
            AddAssistantMessage(history, part.Content ?? "I have no response to that");
        }

        // TODO: Store the interaction in a chat history file for next session

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
