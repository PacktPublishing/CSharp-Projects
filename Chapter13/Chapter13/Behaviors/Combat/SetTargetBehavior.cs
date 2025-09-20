using Chapter13.Entities;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Chapter13.Behaviors.Combat;

public class SetTargetBehavior : IBehavior
{
    public bool CanExecute(SpaceEntityBase entity, GameTime time)
    {
        if (entity is not ShipEntity ship)
        {
            return false;
        }

        // TODO: Will also need to check target alive
        return ship.Target is null && ship.DetectedShips.Any();
    }

    public void Execute(SpaceEntityBase entity, GameTime time)
    {
        ShipEntity ship = (ShipEntity)entity;

        ship.Target = ship.DetectedShips.First();
    }
}
