namespace ModelContextProtocol.ApiService.Requests;

public record SearchRequest
{
    public required string Query { get; init; }
}
