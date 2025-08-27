using Microsoft.Xna.Framework;

namespace Chapter13.Domain;

public class MapLocation
{
    public required string Name { get; init; }
    public required Vector2 StartPosition { get; init; }
    public required Faction Owner { get; init; }
    public required float Size { get; init; }
    public required LocationType Type { get; init; }
    public int Id { get; set; }
    public int DefaultTargetLocationId { get; set; }
}