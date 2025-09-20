using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Chapter13.Systems;

public class TargetRenderingSystem(SpaceGame game) : DrawableGameComponent(game)
{
    private readonly SpriteBatch _spriteBatch = new SpriteBatch(game.GraphicsDevice);

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();

        foreach (var entity in game.Entities)
        {
            if (entity.Target is null) continue;

            _spriteBatch.DrawLine(entity.Bounds.Position, entity.Target.Transform.Position, Color.OrangeRed, 2);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
