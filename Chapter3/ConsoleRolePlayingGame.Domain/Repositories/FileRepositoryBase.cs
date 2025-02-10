using System.Text.Json;

namespace ConsoleRolePlayingGame.Domain.Repositories;

public abstract class FileRepositoryBase
{
    protected static List<T> LoadManyFromJsonFile<T>(string fileName)
    {
        string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Could not find {filePath}");
        }
        
        using FileStream stream = File.OpenRead(filePath);
        List<T>? entries = JsonSerializer.Deserialize<List<T>>(stream);
        
        if (entries is null || entries.Count <= 0)
        {
            throw new JsonException($"{filePath} did not contain valid entries");
        }
        
        return entries;
    }
}