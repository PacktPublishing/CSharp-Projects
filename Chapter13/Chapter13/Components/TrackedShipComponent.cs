using Chapter13.Domain;

namespace Chapter13.Components;

/// <summary>
/// Represents a ship that is being tracked by the game and counts against the maximum allowed ships in an area
/// </summary>
public class TrackedShipComponent
{
    public Faction Faction { get; set; }
}