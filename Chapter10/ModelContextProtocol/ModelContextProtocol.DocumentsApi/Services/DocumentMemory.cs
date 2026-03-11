using System.Numerics.Tensors;
using Microsoft.Extensions.AI;

namespace ModelContextProtocol.DocumentsApi.Services;

public sealed class DocumentMemory(
    List<DocumentChunk> chunks,
    IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator)
{
    public async Task<IReadOnlyList<(string Text, string SourceName, float Score)>> SearchAsync(
        string query, int limit = 5, float minRelevance = 0.1f)
    {
        var queryEmbedding = await embeddingGenerator.GenerateAsync(query);

        var scored = new List<(DocumentChunk Chunk, float Score)>();
        foreach (var chunk in chunks)
        {
            float score = TensorPrimitives.CosineSimilarity(
                queryEmbedding.Vector.Span, chunk.Embedding.Span);

            if (score >= minRelevance)
                scored.Add((chunk, score));
        }

        return scored
            .OrderByDescending(pair => pair.Score)
            .Take(limit)
            .Select(pair => (pair.Chunk.Text, pair.Chunk.SourceName, pair.Score))
            .ToList();
    }
}
