namespace ModelContextProtocol.Domain.Requests;

public record ChatRequest
{
    public required IEnumerable<ApiChatMessage> Messages { get; init; }
}