using Chapter13.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter13.Behaviors.Combat;

public class SetTargetBehavior : IBehavior
{
    public bool CanExecute(ShipEntity ship)
    {
        // TODO: Will also need to check target alive
        return ship.Target is null && ship.DetectedShips.Any();
    }

    public void Execute(ShipEntity ship, GameTime time)
    {
        ship.Target = ship.DetectedShips.First();
    }
}
