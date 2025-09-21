using Microsoft.Xna.Framework;
using MonoGame.Extended.Collisions;

namespace Chapter13.Entities;

public class MissileEntity(SpaceGame game) : SpaceEntityBase
{
    public override float MaxSpeed => 35f;
    public override float MaxTurnRate => 0.25f;
    public override float DetectionRadius => 100f;
    private float _lifeTime = 5f; // seconds

    public override void Update(GameTime gameTime)
    {
        _lifeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_lifeTime <= 0f)
        {
            game.QueueDespawn(this);
        }
        base.Update(gameTime);
    }

    public override void OnCollision(CollisionEventArgs collisionInfo)
    {
        base.OnCollision(collisionInfo);

        if (collisionInfo.Other is SpaceEntityBase otherEntity)
        {
            game.QueueDespawn(this);
            
            if (otherEntity is ShipEntity)
            {
                game.QueueDespawn(otherEntity);
            }

            // TODO: Spawn explosion effect
        }
    }
}