using Microsoft.Xna.Framework;

namespace Chapter13.Components;

public class EngineComponent
{
    public float MaxSpeed { get; set; }
    public float MaxTurnRate { get; set; }
    public Vector2? TargetLocation { get; set; }
}