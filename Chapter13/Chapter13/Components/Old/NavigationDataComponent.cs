using Microsoft.Xna.Framework;

namespace Chapter13.Components;

public class NavigationDataComponent
{
    public Vector2? TargetLocation { get; set; }
    public int? TargetEntityId { get; set; }
}