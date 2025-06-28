using Microsoft.KernelMemory;

namespace ModelContextProtocol.DocumentsApi.Services;

public class DocumentIndexingService(IKernelMemory memory, ILogger<DocumentIndexingService> logger) : IHostedService
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
            string docId = await memory.ImportDocumentAsync(file.FullName, file.Name, cancellationToken: cancellationToken);
            logger.LogInformation("Imported as {id}", docId);
        }

        logger.LogInformation("Finished importing data files");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}