using System.Text.Json;
using System.Text.Json.Serialization;
using CardTracker.Contracts.Responses;

namespace CardTracker.Contracts.Converters;

public class CardResponseConverter : JsonConverter<CardResponse>
{
    public override CardResponse? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using JsonDocument doc = JsonDocument.ParseValue(ref reader);
        
        // Figure out which type of card this is
        JsonElement root = doc.RootElement;
        if (!root.TryGetProperty("type", out JsonElement discriminator))
        {
            throw new JsonException("Missing type property");
        }
        string type = discriminator.GetString()!;
        string json = root.GetRawText();
        
        // Deserialize the correct type
        return type.ToUpperInvariant() switch
        {
            "CREATURE" => JsonSerializer.Deserialize<CreatureCardResponse>(json, options)!,
            "ACTION" => JsonSerializer.Deserialize<ActionCardResponse>(json, options)!,
            _ => throw new JsonException($"Unknown type: {type}")
        };
    }

    public override void Write(Utf8JsonWriter writer, CardResponse value, JsonSerializerOptions options)
    {
        // Serialize the correct type
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}