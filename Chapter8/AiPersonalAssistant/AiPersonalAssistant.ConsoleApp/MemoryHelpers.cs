using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiPersonalAssistant.ConsoleApp;

public class MemoryHelpers
{
    public static async Task<MemoryPlugin> CreateKernelMemoryAsync(AlfredOptions options, IAnsiConsole console)
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
        console.WriteLine("Initializing Memory");
        HashSet<string> supported = new(StringComparer.OrdinalIgnoreCase)
        {
            ".txt", ".docx", ".pdf"
        };
        DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory);
        string[] documentFiles = dir.GetFiles()
            .Where(file => supported.Contains(file.Extension))
            .Select(f => f.FullName)
            .ToArray();

        await console.Progress().StartAsync(async context =>
        {
            foreach (var file in documentFiles.AsParallel())
            {
                FileInfo fileInfo = new(file);
                ProgressTask task = context.AddTask($"Parsing {fileInfo.Name}");
                task.IsIndeterminate = true;
                await mem.ImportDocumentAsync(file);
                task.Increment(100); // Mark the task as complete
            }

        });

        return new MemoryPlugin(mem);
    }
}
