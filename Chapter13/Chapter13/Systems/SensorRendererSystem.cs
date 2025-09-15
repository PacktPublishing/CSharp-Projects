using System.Linq;
using Chapter13.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Chapter13.Systems;

public class SensorRendererSystem(SpaceGame game) : DrawableGameComponent(game)
{
    private readonly SpriteBatch _spriteBatch = new(game.GraphicsDevice);

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();
        foreach (var ship in game.Ships)
        {
            foreach (var sensors in ship.Components.OfType<SensorsComponent>())
            {
                Color color = sensors.HasContact ? Color.Red : Color.Cyan;
                _spriteBatch.DrawCircle(ship.Bounds.Position, sensors.DetectionRadius, sides: 32, color);
            }
        }
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}