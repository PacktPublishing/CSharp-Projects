namespace ModelContextProtocol.ServerShared;

public record McpServerSettings
{
    public required string DocumentsApiEndpoint { get; init; }
}
