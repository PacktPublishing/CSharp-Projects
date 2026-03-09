namespace AiPersonalAssistant.ConsoleApp.Plugins;

/// <summary>
/// Represents a single chunk of text from a source document,
/// along with its pre-computed embedding vector for similarity search.
/// </summary>
public sealed record DocumentChunk
{
    public string Id { get; init; } = Guid.NewGuid().ToString();

    /// <summary>The paragraph / chunk text.</summary>
    public string Text { get; init; } = string.Empty;

    /// <summary>The filename the chunk was extracted from.</summary>
    public string SourceName { get; init; } = string.Empty;

    /// <summary>Pre-computed embedding vector (e.g. 768-dimensional from nomic-embed-text).</summary>
    public ReadOnlyMemory<float> Embedding { get; init; }
}
