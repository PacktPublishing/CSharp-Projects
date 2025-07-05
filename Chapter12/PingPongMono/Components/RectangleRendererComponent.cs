using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PingPongMono.Entities;

namespace PingPongMono.Components;

public class RectangleRendererComponent(Color color) : IPingPongComponent
{
    public void Draw(IPingPongEntity entity, SpriteBatch spriteBatch, PingPongContext context)
    {
        spriteBatch.Draw(context.WhitePixel, entity.Bounds, color);
    }
}