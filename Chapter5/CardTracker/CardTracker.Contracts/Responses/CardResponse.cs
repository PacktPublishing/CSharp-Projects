using System.Text.Json.Serialization;
using CardTracker.Contracts.Converters;

namespace CardTracker.Contracts.Responses;

[JsonConverter(typeof(CardResponseConverter))]
public abstract class CardResponse
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public string? ImagePath { get; init; }
    public abstract string Type { get; }
}