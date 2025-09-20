using Chapter13.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Linq;

namespace Chapter13.Systems;

public class WaypointRenderingSystem(SpaceGame game) : DrawableGameComponent(game)
{
    private readonly SpriteBatch _spriteBatch = new SpriteBatch(game.GraphicsDevice);

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();

        foreach (var ship in game.Entities.OfType<ShipEntity>())
        {
            if (ship.Waypoint is null) continue;

            _spriteBatch.DrawLine(ship.Bounds.Position, ship.Waypoint.Value, Color.Blue, 2);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
