using Chapter13.Entities;
using Chapter13.Helpers;
using Microsoft.Xna.Framework;

namespace Chapter13.Behaviors.Waypoints;

public class SteerTowardsWaypointBehavior : IBehavior
{
    public bool CanExecute(SpaceEntityBase entity, GameTime time)
    {
        if (entity is not ShipEntity ship) return false;

        return ship.Waypoint is not null;
    }

    public void Execute(SpaceEntityBase entity, GameTime time)
    {
        ShipEntity ship = (ShipEntity)entity;
        ship.Transform.RotateTowardsTarget(ship.Waypoint.Value, ship.MaxTurnRate, time);
    }
}