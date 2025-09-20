using Chapter13.Behaviors;
using Microsoft.Xna.Framework;

namespace Chapter13.Entities;

public class ShipEntity : SpaceEntityBase
{
    public Vector2? Waypoint { get; set; }

    public override float MaxSpeed => 15f;
    public override float MaxTurnRate => 1f;
    public override float DetectionRadius => 250f;
}