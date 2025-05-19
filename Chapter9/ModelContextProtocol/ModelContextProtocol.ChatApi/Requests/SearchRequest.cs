namespace ModelContextProtocol.ChatApi.Requests;

public record SearchRequest
{
    public required string Query { get; init; }
}