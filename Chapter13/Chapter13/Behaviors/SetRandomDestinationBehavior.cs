using Chapter13.Entities;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;

namespace Chapter13.Behaviors;

public class SetRandomWaypointBehavior(RectangleF worldBounds) : IBehavior
{
    private readonly Random _random = Random.Shared;

    public bool CanExecute(ShipEntity ship)
    {
        return ship.Waypoint is null;
    }

    public void Execute(ShipEntity ship, GameTime time)
    {
        float x = _random.Next((int)worldBounds.Left, (int)worldBounds.Right);
        float y = _random.Next((int)worldBounds.Top, (int)worldBounds.Bottom);
        ship.Waypoint = new Vector2(x, y);
    }
}