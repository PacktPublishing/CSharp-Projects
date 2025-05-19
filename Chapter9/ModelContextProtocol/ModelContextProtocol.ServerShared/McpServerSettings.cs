namespace ModelContextProtocol.ServerShared;

public record McpServerSettings
{
    public required string KernelMemoryEndpoint { get; init; }
}
