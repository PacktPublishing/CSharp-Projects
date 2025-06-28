namespace ModelContextProtocol.Domain.Requests;

public record SearchRequest
{
    public required string Query { get; init; }
}