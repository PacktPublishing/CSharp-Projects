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
    Kernel kernel = Kernel.CreateBuilder()
        .AddOllamaChatCompletion(options.ChatModelId, ollamaEndpoint)
        .AddOllamaTextEmbeddingGeneration(options.EmbeddingModelId, ollamaEndpoint)
        .AddInMemoryVectorStore()
        .Build();

    await console.Status().StartAsync("Loading memory...", async _ =>
    {
        OllamaConfig config = new()
        {
            Endpoint = options.Endpoint,
            TextModel = new OllamaModelConfig(options.ChatModelId),
            EmbeddingModel = new OllamaModelConfig(options.EmbeddingModelId) // 2048
        };

        IKernelMemory mem = new KernelMemoryBuilder()
            .WithOllamaTextGeneration(config)
            .WithOllamaTextEmbeddingGeneration(config)
            .Build<MemoryServerless>();

        // Find all files with extensions of .txt, .docx, and .pdf in the directory
        string[] documentFiles = Directory.GetFiles(
                Environment.CurrentDirectory,
                "*.*",
                SearchOption.TopDirectoryOnly
            )
            .Where(file =>
                file.EndsWith(".txt", StringComparison.OrdinalIgnoreCase) ||
                file.EndsWith(".docx", StringComparison.OrdinalIgnoreCase) ||
                file.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase)
            )
            .ToArray();

        foreach (var file in documentFiles)
        {
            await mem.ImportDocumentAsync(file);
        }

        kernel.ImportPluginFromObject(new MemoryPlugin(mem));
    });

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

        if (exitWords.Contains(reply)) break;

        IReadOnlyList<ChatMessageContent> response = [];
        await console.Status().StartAsync("Thinking...",
            async _ => { response = await chatService.GetChatMessageContentsAsync(history, settings, kernel); });

        foreach (var part in response)
        {
            AddAssistantMessage(history, part.Content ?? "I have no response to that");
        }
    } while (!string.IsNullOrWhiteSpace(reply));

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
