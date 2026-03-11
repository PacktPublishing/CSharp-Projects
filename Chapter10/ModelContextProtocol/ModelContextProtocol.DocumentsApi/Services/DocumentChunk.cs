namespace ModelContextProtocol.DocumentsApi.Services;

public sealed record DocumentChunk
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string Text { get; init; } = string.Empty;
    public string SourceName { get; init; } = string.Empty;
    public ReadOnlyMemory<float> Embedding { get; init; }
}
