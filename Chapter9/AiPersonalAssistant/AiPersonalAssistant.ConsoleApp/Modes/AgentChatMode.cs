namespace AiPersonalAssistant.ConsoleApp.Modes;

public class AgentChatMode(IAnsiConsole console) : AlfredChatHandler(console)
{
    private AIAgent? _agent;
    private AgentSession? _session;

    public override async Task InitializeAsync(AlfredOptions options)
    {
        var chatClient = new OllamaChatClient(
            new Uri(options.Endpoint), options.ChatModelId);

        _agent = chatClient.AsAIAgent(
            instructions: options.SystemPrompt,
            tools: GetTools());

        _session = await _agent.CreateSessionAsync();

        await base.InitializeAsync(options);
    }

    public virtual List<AITool> GetTools()
    {
        return [AIFunctionFactory.Create(TimeAndDatePlugin.GetCurrentTimeAndDate)];
    }

    public override async Task ChatAsync(string message)
    {
        AgentResponse response = await _agent!.RunAsync(message, _session!);

        AddAssistantMessage(response.Text ?? "I have no response to that.");
    }
}
