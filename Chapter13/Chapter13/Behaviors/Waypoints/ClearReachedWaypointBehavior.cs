using Chapter13.Entities;
using Microsoft.Xna.Framework;

namespace Chapter13.Behaviors.Waypoints;

public class ClearReachedWaypointBehavior : IBehavior
{
    public const float ReachedWaypointDistance = 10f;

    public bool CanExecute(ShipEntity ship)
    {
        if (ship.Waypoint is null) return false;

        float distance = Vector2.Distance(ship.Bounds.Position, ship.Waypoint.Value);

        return distance <= ReachedWaypointDistance;
    }

    public void Execute(ShipEntity ship, GameTime time)
    {
        ship.Waypoint = null;
    }
}
