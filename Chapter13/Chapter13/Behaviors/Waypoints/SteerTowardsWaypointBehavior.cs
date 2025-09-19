using Chapter13.Entities;
using Chapter13.Helpers;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;

namespace Chapter13.Behaviors.Waypoints;

public class SteerTowardsWaypointBehavior : IBehavior
{
    public bool CanExecute(ShipEntity ship)
        => ship.Waypoint is not null;

    public void Execute(ShipEntity ship, GameTime time)
    {
        ship.Transform.RotateTowardsTarget(ship.Waypoint.Value, ship.MaxTurnRate, time);
    }
}