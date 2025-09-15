using Chapter13.Entities;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace Chapter13.Components;

public class SensorsComponent(ShipEntity ship) : Updateable, ICollisionActor
{
    public float DetectionRadius { get; set; } = 100f;
    public void OnCollision(CollisionEventArgs collisionInfo)
    {
        HasContact = true;
    }

    public bool HasContact { get; set; }

    public override void Update(GameTime gameTime)
    {
        HasContact = false;
    }

    public IShapeF Bounds => new CircleF(ship.Transform.Position, DetectionRadius);
}