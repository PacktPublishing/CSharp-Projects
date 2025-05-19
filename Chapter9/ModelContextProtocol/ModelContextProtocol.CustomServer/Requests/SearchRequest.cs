namespace ModelContextProtocol.CustomServer.Requests;

public record SearchRequest
{
    public required string Query { get; init; }
}
