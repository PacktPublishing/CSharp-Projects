using Chapter13.Entities;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;

namespace Chapter13.Behaviors;

public class SteerTowardsWaypointBehavior : IBehavior
{
    public bool CanExecute(ShipEntity ship) 
        => ship.Waypoint is not null;

    public void Execute(ShipEntity ship, GameTime time)
    {
        Transform2 transform = ship.Transform;
        Vector2 position = transform.Position;
        Vector2 waypoint = ship.Waypoint.Value;
        float desiredAngle = (float)Math.Atan2(waypoint.Y - position.Y, waypoint.X - transform.Position.X);
        float angleDifference = MathHelper.WrapAngle(desiredAngle - transform.Rotation);
        float maxTurn = ship.MaxTurnRate * (float)time.ElapsedGameTime.TotalSeconds;
        angleDifference = MathHelper.Clamp(angleDifference, -maxTurn, maxTurn);
        transform.Rotation += angleDifference;
    }
}