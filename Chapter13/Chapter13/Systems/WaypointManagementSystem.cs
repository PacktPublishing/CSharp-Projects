using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;

namespace Chapter13.Systems;

public class WaypointManagementSystem(RectangleF worldBounds, SpaceGame game) : DrawableGameComponent(game)
{
    private readonly SpriteBatch _spriteBatch = new SpriteBatch(game.GraphicsDevice);
    private readonly Random _random = Random.Shared;
    public const float ReachedWaypointDistance = 5f;

    public override void Update(GameTime gameTime)
    {
        foreach (var ship in game.Ships)
        {
            if (ship.Waypoint != null)
            {
                // Get the distance to the waypoint
                float distance = Vector2.Distance(ship.Bounds.Position, ship.Waypoint.Value);

                if (distance <= ReachedWaypointDistance)
                {
                    ship.Waypoint = null;
                }
            }

            if (ship.Waypoint == null)
            {
                float x = _random.Next((int)worldBounds.Left, (int)worldBounds.Right);
                float y = _random.Next((int)worldBounds.Top, (int)worldBounds.Bottom);
                ship.Waypoint = new Vector2(x, y);
            }
        }

        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();

        foreach (var ship in game.Ships)
        {
            if (ship.Waypoint is null) continue;

            _spriteBatch.DrawLine(ship.Bounds.Position, ship.Waypoint.Value, Color.Yellow, 2);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
