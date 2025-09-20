using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;

namespace Chapter13.Systems;

public class SpriteRendererSystem(SpaceGame game) : DrawableGameComponent(game)
{
    private readonly SpriteBatch _spriteBatch = new(game.GraphicsDevice);

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();
        foreach (var ship in game.Ships)
        {
            _spriteBatch.Draw(ship.Sprite, ship.Transform);
        }
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}