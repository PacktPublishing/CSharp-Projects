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
        ApplicationMode.SemanticKernel => new SemanticKernelChatMode(console),
        ApplicationMode.Combined => new CombinedChatMode(console),
        _ => throw new NotSupportedException()
    };

    await handler.InitializeAsync(options);

    /*
    kernel.ImportPluginFromObject(new ReadOnlyKernelMemoryPlugin(memory, console));
    */

    HashSet<string> exitWords = new(StringComparer.OrdinalIgnoreCase) {
        "exit", "quit", "q", "e", "x", "bye", "goodbye"
    };

    handler.AddAssistantMessage(options.GreetingMessage);

    // Main conversation loop
    string? reply;
    TextPrompt<string> userPrompt = new("[orange3]User[/]: ");
    do
    {
        reply = console.Prompt(userPrompt);
        if (exitWords.Contains(reply)) break;

        await handler.ChatAsync(reply);
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