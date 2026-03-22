using Chapter13.Entities;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;

namespace Chapter13.Behaviors.Waypoints;

public class SetRandomWaypointBehavior(RectangleF worldBounds) : IBehavior
{
    private readonly Random _random = Random.Shared;

    public bool CanExecute(SpaceEntityBase entity, GameTime time)
    {
        if (entity is not ShipEntity ship) return false;

        return ship.Waypoint is null;
    }

    public void Execute(SpaceEntityBase entity, GameTime time)
    {
        ShipEntity ship = (ShipEntity)entity;
        float x = _random.Next((int)worldBounds.Left, (int)worldBounds.Right);
        float y = _random.Next((int)worldBounds.Top, (int)worldBounds.Bottom);
        ship.Waypoint = new Vector2(x, y);
    }
}