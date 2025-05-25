namespace AiPersonalAssistant.ConsoleApp.Plugins;

public class MemoryHelpers
{
    public static async Task<IKernelMemory> LoadMemory(AlfredOptions options, IAnsiConsole console)
    {
        console.MarkupLine("[orange3]Initializing Memory[/]");

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

        console.MarkupLineInterpolated($"Searching for documents in [yellow]{options.DataDirectory}[/]");
        string directory = GetEffectiveDirectory(options);
        string[] documentFiles = FindDocuments(directory);

        await console.Progress().StartAsync(async context =>
        {
            foreach (var file in documentFiles.AsParallel())
            {
                FileInfo fileInfo = new(file);
                ProgressTask task = context.AddTask(fileInfo.Name);
                task.IsIndeterminate = true;
                await mem.ImportDocumentAsync(file);
                task.Increment(100); // Mark the task as complete
            }
        });

        return mem;
    }

    private static string[] FindDocuments(string directory)
    {
        HashSet<string> supported = new(StringComparer.OrdinalIgnoreCase)
        {
            ".txt", ".docx", ".pdf", ".md", ".html", ".htm", ".pptx"
        };

        DirectoryInfo dir = new DirectoryInfo(directory);
        return dir.GetFiles()
            .Where(file => supported.Contains(file.Extension))
            .Select(f => f.FullName)
            .ToArray();
    }

    private static string GetEffectiveDirectory(AlfredOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.DataDirectory))
        {
            return Environment.CurrentDirectory;
        }

        string path = options.DataDirectory;
        if (!Path.IsPathRooted(path))
        {
            path = Path.Combine(Environment.CurrentDirectory, path);
        }

        return path;
    }
}