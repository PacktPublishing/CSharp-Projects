using Microsoft.Xna.Framework;
using MonoGame.Extended.Collisions;

namespace Chapter13.Entities;

public class MissileEntity(SpaceGame game, SpaceEntityBase owner) : SpaceEntityBase
{
    public override float MaxSpeed => 50f;
    public override float MaxTurnRate => 0.25f;
    public override float DetectionRadius => 100f;
    private float _lifeTime = 7f;

    public override void Update(GameTime gameTime)
    {
        if (_lifeTime <= 0f)
        {
            game.QueueDespawn(this);
        }
        else
        {
            _lifeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        base.Update(gameTime);
    }

    public override void OnCollision(CollisionEventArgs collisionInfo)
    {
        base.OnCollision(collisionInfo);

        // Ensure we're not colliding with the originating ship
        if (collisionInfo.Other != owner)
        {
            game.QueueDespawn(this);
            
            if (collisionInfo.Other is ShipEntity otherShip)
            {
                game.QueueDespawn(otherShip);
            }

            // TODO: Spawn explosion effect
        }
    }
}