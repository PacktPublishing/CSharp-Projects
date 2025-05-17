namespace AiPersonalAssistant.ConsoleApp.Modes;

public class CombinedChatMode(IAnsiConsole console) : SemanticKernelChatMode(console)
{
    [Experimental("SKEXP0070")]
    public override async Task InitializeAsync(AlfredOptions options)
    {
        await base.InitializeAsync(options);

        IKernelMemory memory = await MemoryHelpers.LoadKernelMemoryAsync(options, Console);
        Kernel!.ImportPluginFromObject(new ReadOnlyKernelMemoryPlugin(memory, Console));
    }
}
