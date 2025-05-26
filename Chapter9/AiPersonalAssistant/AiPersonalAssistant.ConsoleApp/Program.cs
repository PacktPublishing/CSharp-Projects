// Write header
IAnsiConsole console = AnsiConsole.Console;
console.Write(new FigletText("Alfred").Color(Color.Yellow));
console.MarkupLine("[aqua]Digital butler to the budding super-coder[/]");
console.WriteLine();

// Load options
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>()
    .AddCommandLine(args)
    .Build();
AlfredOptions options = config.Get<AlfredOptions>()!;

// Set up our application
ApplicationMode mode = console.Prompt(new SelectionPrompt<ApplicationMode>()
    .AddChoices(Enum.GetValues<ApplicationMode>())
    .Title("Select an application mode"));

AlfredChatHandler handler = mode switch
{
    ApplicationMode.KernelMemorySearch => new KernelMemorySearchMode(console),
    ApplicationMode.KernelMemoryChat => new KernelMemoryChatMode(console),
    ApplicationMode.SemanticKernel => new SemanticKernelChatMode(console),
    ApplicationMode.Combined => new CombinedChatMode(console),
    _ => throw new NotSupportedException()
};

await handler.InitializeAsync(options);

HashSet<string> exitWords = new(StringComparer.OrdinalIgnoreCase) {
    "exit", "quit", "q", "e", "x", "bye", "goodbye"
};

// Main conversation loop
string? reply;
TextPrompt<string> userPrompt = new("[orange3]User[/]: ");
do
{
    reply = console.Prompt(userPrompt);
    if (exitWords.Contains(reply))
        break;

    await handler.ChatAsync(reply);
} while (!string.IsNullOrWhiteSpace(reply));

handler.AddAssistantMessage(options.GoodbyeMessage);
