namespace AiPersonalAssistant.ConsoleApp.Modes;

public class CombinedChatMode(IAnsiConsole console) : SemanticKernelChatMode(console)
{
    private IKernelMemory? _memory;

    [Experimental("SKEXP0070")]
    public override async Task InitializeAsync(AlfredOptions options)
    {
        _memory = await MemoryHelpers.LoadMemory(options, Console);

        await base.InitializeAsync(options);
    }

    public override void LoadPlugins(Kernel kernel)
    {
        base.LoadPlugins(kernel);

        if (_memory is null) throw new InvalidOperationException("Memory not set");

        ReadOnlyKernelMemoryPlugin plugin = new(_memory, Console);
        kernel.ImportPluginFromObject(plugin);
    }
}
