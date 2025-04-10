using System.Text.Json;
using System.Text.Json.Serialization;
using CardTracker.Contracts.Requests;

namespace CardTrackerWebApi.Converters;

public class CreateCardRequestConverter : JsonConverter<CreateCardRequest>
{
    public override CreateCardRequest Read(ref Utf8JsonReader reader, Type convertType, JsonSerializerOptions options)
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
            "CREATURE" => JsonSerializer.Deserialize<CreateCreatureCardRequest>(json, options)!,
            "ACTION" => JsonSerializer.Deserialize<CreateActionCardRequest>(json, options)!,
            _ => throw new JsonException($"Unknown type: {type}")
        };
    }

    public override void Write(Utf8JsonWriter writer, CreateCardRequest value, JsonSerializerOptions options) 
        => JsonSerializer.Serialize(writer, (object)value, value.GetType(), options);
}