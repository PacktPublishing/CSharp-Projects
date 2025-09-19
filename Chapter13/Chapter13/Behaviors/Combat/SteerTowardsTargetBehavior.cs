using Chapter13.Entities;
using Chapter13.Helpers;
using Microsoft.Xna.Framework;

namespace Chapter13.Behaviors.Combat;

public class SteerTowardsTargetBehavior : IBehavior
{
    public bool CanExecute(ShipEntity ship)
    {
        return ship.Target is not null;
    }

    public void Execute(ShipEntity ship, GameTime time)
    {
        ship.Transform.RotateTowardsTarget(ship.Target.Transform.Position, ship.MaxTurnRate, time);
    }
}