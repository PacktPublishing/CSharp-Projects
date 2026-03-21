namespace AiPersonalAssistant.ConsoleApp.Tools;

public class MemoryHelpers
{
    public static async Task<DocumentMemory> LoadMemory(AlfredOptions options, IAnsiConsole console)
    {
        console.MarkupLine("[orange3]Initializing Memory[/]");

        var embeddingGenerator = new OllamaEmbeddingGenerator(
            new Uri(options.Endpoint), options.EmbeddingModelId);

        // Plain list — no external vector store needed for small document sets
        var chunks = new List<DocumentChunk>();

        console.MarkupLineInterpolated($"Searching for documents in [yellow]{options.DataDirectory}[/]");
        string directory = GetEffectiveDirectory(options);
        string[] documentFiles = FindDocuments(directory);

        await console.Progress().StartAsync(async context =>
        {
            foreach (var file in documentFiles)
            {
                FileInfo fileInfo = new(file);
                ProgressTask task = context.AddTask(fileInfo.Name);
                task.IsIndeterminate = true;

                var paragraphs = ChunkFile(file);
                foreach (var paragraph in paragraphs)
                {
                    var embedding = await embeddingGenerator.GenerateAsync(paragraph.Text);
                    var record = new DocumentChunk
                    {
                        Text = paragraph.Text,
                        SourceName = paragraph.SourceName,
                        Embedding = embedding.Vector
                    };
                    chunks.Add(record);
                }

                task.Increment(100);
            }
        });

        return new DocumentMemory(chunks, embeddingGenerator);
    }

    private static IEnumerable<(string Text, string SourceName)> ChunkFile(string filePath)
    {
        string text = File.ReadAllText(filePath);
        string sourceName = Path.GetFileName(filePath);

        // Split by double newlines (paragraphs)
        string[] paragraphs = text.Split(["\r\n\r\n", "\n\n"], StringSplitOptions.RemoveEmptyEntries);

        foreach (string para in paragraphs)
        {
            string trimmed = para.Trim();
            if (trimmed.Length > 20) // Skip very short fragments
            {
                yield return (trimmed, sourceName);
            }
        }
    }

    private static string[] FindDocuments(string directory)
    {
        HashSet<string> supported = new(StringComparer.OrdinalIgnoreCase)
        {
            ".txt", ".md", ".html", ".htm"
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