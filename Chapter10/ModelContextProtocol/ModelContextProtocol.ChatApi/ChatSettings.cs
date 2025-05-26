namespace ModelContextProtocol.ChatApi;

public class ChatSettings
{
    public required string SystemPrompt { get; init; }
    public required string ChatModelId { get; init; }
    public required string ChatEndpoint { get; init; }
    public required string McpServerEndpoint { get; init; }
}
