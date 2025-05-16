using AiPersonalAssistant.ConsoleApp;
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

    // Add KernelMemory as needed
    IKernelMemory? memory = null;
    if (options.UseKernelMemory)
    {
        memory = await MemoryHelpers.LoadKernelMemoryAsync(options, console);
    }

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

    HashSet<string> exitWords = new(StringComparer.OrdinalIgnoreCase) {
        "exit", "quit", "q", "e", "x", "bye", "goodbye"
    };

    ChatHistory history = new(options.SystemPrompt);
    AddAssistantMessage(history, options.GreetingMessage);

    if (options.UseSemanticKernel)
    {
        await ChatWithKernelAsync(kernel!, exitWords, history);
    }
    else if (options.UseKernelMemory)
    {
        await ChatWithKernelMemoryAsync(memory!, exitWords, history);
    }
    else
    {
        console.MarkupLine("[red]Error:[/] You must specify Semantic Kernel, KernelMemory, or both.");
        return -1;
    }

    AddAssistantMessage(history, options.GoodbyeMessage);
    return 0;
}
catch (Exception ex)
{
    console.WriteException(ex, ExceptionFormats.ShortenEverything);
    return -2;
}

void DisplayAppHeader(IAnsiConsole ansiConsole)
{
    Style accentStyle = new Style(Color.Aqua);

    ansiConsole.Write(new FigletText("Alfred").Color(Color.Yellow));
    ansiConsole.WriteLine("Digital butler to the budding super-coder", accentStyle);
    ansiConsole.WriteLine();
}

async Task ChatWithKernelMemoryAsync(IKernelMemory kernelMemory, HashSet<string> hashSet, ChatHistory history)
{
    string reply;
    do
    {
        reply = GetUserMessage(console, history);
        if (hashSet.Contains(reply))
            break;

        MemoryAnswer response = await kernelMemory.AskAsync(reply);

        console.MarkupLine("[cyan]RAG Search Results:[/]");
        string json = response.ToJson(optimizeForStream: false);
        console.Write(new JsonText(json));

        AddAssistantMessage(history, response.Result);
    } while (!string.IsNullOrWhiteSpace(reply));

}

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

void AddAssistantMessage(ChatHistory chatHistory, string message)
{
    chatHistory.AddAssistantMessage(message);
    console.MarkupLineInterpolated($"[yellow]Alfred[/]: {message}");
    // TODO: Store the interaction in a chat history file for next session
}

string GetUserMessage(IAnsiConsole ansiConsole, ChatHistory chatHistory)
{
    var message = ansiConsole.Prompt(new TextPrompt<string>("[orange3]User[/]: "));
    chatHistory.AddUserMessage(message);

    // TODO: Store the interaction

    return message;
}

#pragma warning restore SKEXP0070
