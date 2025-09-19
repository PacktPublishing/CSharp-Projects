using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Chapter13.Systems;

public class WaypointManagementSystem(SpaceGame game) : DrawableGameComponent(game)
{
    private readonly SpriteBatch _spriteBatch = new SpriteBatch(game.GraphicsDevice);

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
