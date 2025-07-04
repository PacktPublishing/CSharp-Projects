using Microsoft.Xna.Framework.Graphics;

namespace PingPongMono;

public interface IDrawable
{
    void Draw(SpriteBatch spriteBatch, PingPongContext context);
}