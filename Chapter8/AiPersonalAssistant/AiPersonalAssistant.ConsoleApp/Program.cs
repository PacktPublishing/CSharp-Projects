#pragma warning disable SKEXP0070
IAnsiConsole console = AnsiConsole.Console;

try
{
    AlfredOptions options = new();

    Console.OutputEncoding = Encoding.Unicode;
    Console.InputEncoding = Encoding.Unicode;

    DisplayAppHeader(console);

    // Build the kernel
    Kernel kernel = Kernel.CreateBuilder()
        .AddOllamaChatCompletion(options.ModelId, options.Endpoint)
        .Build();
    IChatCompletionService chatService = kernel.GetRequiredService<IChatCompletionService>();

    // Initialize chat history
    ChatHistory history = new(options.SystemPrompt);
    AddAssistantMessage(history, "Hello. How may I help you today? Up to any coding sprees?");

    HashSet<string> exitWords = new(StringComparer.OrdinalIgnoreCase) {
        "exit", "quit", "q", "e", "x", "bye", "goodbye"
    };

    // Main conversation loop
    string? reply;
    do
    {
        reply = console.Prompt(new TextPrompt<string>("[orange3]User[/]: "));
        history.AddUserMessage(reply);

        IReadOnlyList<ChatMessageContent> response = [];
        await console.Status().StartAsync("Thinking...",
            async _ => { response = await chatService.GetChatMessageContentsAsync(history); });

        foreach (var part in response)
        {
            AddAssistantMessage(history, part.Content ?? "I have no response to that");
        }
    } while (!string.IsNullOrWhiteSpace(reply) && !exitWords.Contains(reply));

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
