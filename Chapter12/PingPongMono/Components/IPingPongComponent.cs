using Microsoft.Xna.Framework.Graphics;
using PingPongMono.Entities;

namespace PingPongMono.Components;

public interface IPingPongComponent
{
    void Update(IPingPongEntity entity, PingPongContext context)
    {
        // Default implementation does nothing
    }

    void Draw(IPingPongEntity entity, SpriteBatch spriteBatch, PingPongContext context)
    {
        // Default implementation does nothing
    }
}