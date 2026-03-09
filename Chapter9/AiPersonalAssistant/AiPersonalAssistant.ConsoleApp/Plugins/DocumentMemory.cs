namespace AiPersonalAssistant.ConsoleApp.Plugins;

/// <summary>
/// A lightweight in-memory document store that supports semantic search.
/// Uses brute-force cosine similarity via <see cref="TensorPrimitives"/> —
/// ideal for small corpora (hundreds of chunks) where an external vector
/// database would be overkill.
/// </summary>
public sealed class DocumentMemory(
    List<DocumentChunk> chunks,
    IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator)
{
    /// <summary>
    /// Embeds the <paramref name="query"/> and returns the most relevant chunks
    /// ranked by cosine similarity.
    /// </summary>
    /// <param name="query">Natural-language search query.</param>
    /// <param name="limit">Maximum number of results to return.</param>
    /// <param name="minRelevance">Minimum cosine-similarity score (0–1) to include a result.</param>
    public async Task<IReadOnlyList<(string Text, string SourceName, float Score)>> SearchAsync(
        string query, int limit = 5, float minRelevance = 0.1f)
    {
        // 1. Generate an embedding for the search query
        var queryEmbedding = await embeddingGenerator.GenerateAsync(query);

        // 2. Score every chunk using cosine similarity (TensorPrimitives is part of .NET)
        var scored = new List<(DocumentChunk Chunk, float Score)>();
        foreach (var chunk in chunks)
        {
            float score = TensorPrimitives.CosineSimilarity(
                queryEmbedding.Vector.Span, chunk.Embedding.Span);

            if (score >= minRelevance)
                scored.Add((chunk, score));
        }

        // 3. Return the top results sorted by relevance
        return scored
            .OrderByDescending(pair => pair.Score)
            .Take(limit)
            .Select(pair => (pair.Chunk.Text, pair.Chunk.SourceName, pair.Score))
            .ToList();
    }
}
