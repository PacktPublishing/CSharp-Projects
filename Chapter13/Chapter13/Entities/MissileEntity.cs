using Microsoft.Xna.Framework;

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
            game.Despawn(this);
        }
        base.Update(gameTime);
    }
}