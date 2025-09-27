using Chapter13.Entities;
using Chapter13.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter13.Behaviors.Combat;

public class SteerAwayFromMissileBehavior : IBehavior
{
    public bool CanExecute(SpaceEntityBase ship, GameTime time)
    {
        return ship.DetectedEntities.Any(e => e is MissileEntity);
    }

    public void Execute(SpaceEntityBase ship, GameTime time)
    {
        // Find the first detected missile
        MissileEntity missile = ship.DetectedEntities.OfType<MissileEntity>().First();

        ship.Transform.RotateAwayFromTarget(missile.Transform.Position, ship.MaxTurnRate, time);
    }
}
