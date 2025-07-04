using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PingPongMono.Entities;

public class Paddle(int x, int y, int width, int height, Color color) 
    : PingPongEntity(new Rectangle(x, y, width, height))
{
    public override void Draw(SpriteBatch spriteBatch, PingPongContext context)
    {
        spriteBatch.Draw(context.WhitePixel, Bounds, color);
    }
}