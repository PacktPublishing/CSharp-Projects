namespace ModelContextProtocol.CustomServer;

public record McpServerSettings
{
    public required string KernelMemoryEndpoint { get; init; }
}
