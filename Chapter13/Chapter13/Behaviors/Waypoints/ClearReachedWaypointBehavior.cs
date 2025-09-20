using Chapter13.Entities;
using Microsoft.Xna.Framework;

namespace Chapter13.Behaviors.Waypoints;

public class ClearReachedWaypointBehavior : IBehavior
{
    public const float ReachedWaypointDistance = 10f;

    public bool CanExecute(SpaceEntityBase entity, GameTime time)
    {
        if (entity is not ShipEntity ship) return false;

        if (ship.Waypoint is null) return false;

        float distance = Vector2.Distance(ship.Bounds.Position, ship.Waypoint.Value);

        return distance <= ReachedWaypointDistance;
    }

    public void Execute(SpaceEntityBase entity, GameTime time)
    {
        ShipEntity ship = (ShipEntity)entity;
        ship.Waypoint = null;
    }
}
