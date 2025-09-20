using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Graphics;

namespace Chapter13.Entities;

public abstract class SpaceEntityBase : Updateable, ICollisionActor
{
    public Transform2 Transform { get; } = new();

    public ShipEntity? Target { get; set; }
    public Sprite Sprite { get; internal set; }
    public abstract float MaxTurnRate { get; }

    public abstract IShapeF Bounds { get; }

    public void OnCollision(CollisionEventArgs collisionInfo)
    {
        // No operation needed
    }
}
