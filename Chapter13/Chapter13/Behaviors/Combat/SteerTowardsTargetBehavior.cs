using Chapter13.Entities;
using Chapter13.Helpers;
using Microsoft.Xna.Framework;

namespace Chapter13.Behaviors.Combat;

public class SteerTowardsTargetBehavior : IBehavior
{
    public bool CanExecute(SpaceEntityBase ship, GameTime time)
    {
        return ship.Target is not null;
    }

    public void Execute(SpaceEntityBase ship, GameTime time)
    {
        Vector2 target = ship.Target.Transform.Position;
        ship.Transform.RotateTowardsTarget(target, ship.MaxTurnRate, time);
    }
}