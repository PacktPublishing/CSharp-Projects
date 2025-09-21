using Chapter13.Behaviors;
using Microsoft.Xna.Framework;

namespace Chapter13.Entities;

public class ShipEntity : SpaceEntityBase
{
    public Vector2? Waypoint { get; set; }
    public float TimeUntilReadyToFire { get; set; } = 0f;

    public override float MaxSpeed => 20f;
    public override float MaxTurnRate => 0.7f;
    public override float DetectionRadius => 250f;

    public override void Update(GameTime gameTime)
    {
        if (TimeUntilReadyToFire > 0f)
        {
            TimeUntilReadyToFire -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        base.Update(gameTime);
    }
}