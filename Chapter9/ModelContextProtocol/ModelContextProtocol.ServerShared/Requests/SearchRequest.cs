namespace ModelContextProtocol.ServerShared.Requests;

public record SearchRequest
{
    public required string Query { get; init; }
}
