using ModelContextProtocol.Protocol;

namespace ModelContextProtocol.ChatApi.Requests;

public record ChatRequest
{
    public required IEnumerable<ApiChatMessage> Messages { get; init; }
}

public record ApiChatMessage(Role Role, string Message);