using Microsoft.Extensions.Configuration;
using AiPersonalAssistant.ConsoleApp.Modes;

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

    AlfredChatHandler handler = options.Mode switch
    {
        ApplicationMode.KernelMemorySearch => new KernelMemorySearchMode(console),
        ApplicationMode.KernelMemoryChat => new KernelMemoryChatMode(console),
        _ => throw new NotSupportedException()
    };

    await handler.InitializeAsync(options);

    /*
    // Add Semantic Kernel as needed
    Kernel? kernel = null;
    if (options.UseSemanticKernel)
    {
        kernel = Kernel.CreateBuilder()
            .AddOllamaChatCompletion(options.ChatModelId, new Uri(options.Endpoint))
            .Build();

        if (memory is not null)
        {
            kernel.ImportPluginFromObject(new ReadOnlyKernelMemoryPlugin(memory, console));
        }

        // TODO: Add a bing or google search plugin if a key is specified
        // TODO: Add a simple custom skill for something silly
    }
    */

    HashSet<string> exitWords = new(StringComparer.OrdinalIgnoreCase) {
        "exit", "quit", "q", "e", "x", "bye", "goodbye"
    };

    handler.AddAssistantMessage(options.GreetingMessage);

    // Main conversation loop
    string? reply;
    do
    {
        reply = handler.GetUserMessage();
        if (exitWords.Contains(reply)) break;

        // TODO: Probably don't need this return value
        IAsyncEnumerable<string> responses = handler.ChatAsync(reply);

        await foreach (var response in responses)
        {
            handler.AddAssistantMessage(response);
        }
    } while (!string.IsNullOrWhiteSpace(reply));

    handler.AddAssistantMessage(options.GoodbyeMessage);
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

/*
async Task ChatWithKernelAsync(Kernel sk, HashSet<string> hashSet, ChatHistory history)
{
    // Initialize chat history
    IChatCompletionService chatService = sk.GetRequiredService<IChatCompletionService>();
    PromptExecutionSettings settings = new()
    {
        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
    };

    // Main conversation loop
    string? reply;
    do
    {
        reply = GetUserMessage(console, history);
        if (hashSet.Contains(reply)) break;

        // TODO: Adding logging at a function call level would be helpful

        IReadOnlyList<ChatMessageContent> response = await chatService.GetChatMessageContentsAsync(history, settings, sk);

        foreach (var part in response)
        {
            AddAssistantMessage(history, part.Content ?? "I have no response to that");
        }
    } while (!string.IsNullOrWhiteSpace(reply));
}
*/

#pragma warning restore SKEXP0070
