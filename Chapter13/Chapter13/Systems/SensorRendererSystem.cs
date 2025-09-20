using System.Linq;
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
        foreach (var entity in game.Entities)
        {
            Color color = entity.DetectedEntities.Count() > 0 ? Color.Red : Color.Cyan;
            _spriteBatch.DrawCircle(entity.Bounds.Position, entity.DetectionRadius, sides: 32, color);
        }
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}