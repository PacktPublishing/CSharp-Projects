using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
namespace AiPersonalAssistant.ConsoleApp;

public class ReadOnlyKernelMemoryPlugin(IKernelMemory memory, IAnsiConsole console)
{
    [KernelFunction("Search")]
    [Description("Searches documents and history for answers to a question")]
    public async Task<string> Search(string question)
    {
        console.MarkupLineInterpolated($"[cyan]Memory Search[/]: {question}");

        MemoryAnswer result = await memory.AskAsync(question);

        // TODO: Display the results in diagnostics in tabular form

        return result.Result ?? result.NoResultReason ?? "No answer found";
    }
}
