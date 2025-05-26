namespace ModelContextProtocol.ApiService;

public class OllamaSearchOptions
{
    public required string Endpoint { get; init; }
    public required string ChatModelId { get; init; }
    public required string EmbeddingModelId { get; init; }
}
