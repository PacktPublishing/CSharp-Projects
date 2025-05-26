namespace ModelContextProtocol.DocumentsApi.Requests;

public record SearchRequest
{
    public required string Query { get; init; }
}
