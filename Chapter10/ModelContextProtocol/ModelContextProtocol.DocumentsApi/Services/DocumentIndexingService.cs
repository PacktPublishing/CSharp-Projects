using Microsoft.Extensions.AI;

namespace ModelContextProtocol.DocumentsApi.Services;

public class DocumentIndexingService(
    IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator,
    List<DocumentChunk> chunks,
    ILogger<DocumentIndexingService> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        DirectoryInfo dataDir = new(Path.Combine(Environment.CurrentDirectory, "Data"));
        logger.LogInformation("Looking for data files in {path}", dataDir.FullName);

        if (!dataDir.Exists)
            throw new DirectoryNotFoundException($"{dataDir.FullName} does not exist");

        foreach (var file in dataDir.GetFiles())
        {
            logger.LogInformation("Indexing {document}", file.FullName);

            var paragraphs = ChunkFile(file.FullName);
            foreach (var paragraph in paragraphs)
            {
                var embedding = await embeddingGenerator.GenerateAsync(paragraph.Text, cancellationToken: cancellationToken);
                chunks.Add(new DocumentChunk
                {
                    Text = paragraph.Text,
                    SourceName = paragraph.SourceName,
                    Embedding = embedding.Vector
                });
            }

            logger.LogInformation("Indexed {document}", file.Name);
        }

        logger.LogInformation("Finished importing {count} chunks from data files", chunks.Count);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static IEnumerable<(string Text, string SourceName)> ChunkFile(string filePath)
    {
        string text = File.ReadAllText(filePath);
        string sourceName = Path.GetFileName(filePath);

        string[] paragraphs = text.Split(["\r\n\r\n", "\n\n"], StringSplitOptions.RemoveEmptyEntries);

        foreach (string para in paragraphs)
        {
            string trimmed = para.Trim();
            if (trimmed.Length > 20)
            {
                yield return (trimmed, sourceName);
            }
        }
    }
}