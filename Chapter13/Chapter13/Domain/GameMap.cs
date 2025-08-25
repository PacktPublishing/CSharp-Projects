using System.Collections.Generic;

namespace Chapter13.Domain;

public class GameMap
{
    public required string Name { get; init; }
    public required List<MapLocation> Locations { get; set; }
}