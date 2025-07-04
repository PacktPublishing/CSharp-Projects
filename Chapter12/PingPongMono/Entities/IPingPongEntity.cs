using Microsoft.Xna.Framework.Graphics;

namespace PingPongMono.Entities;

public interface IPingPongEntity
{
    void Update(PingPongContext context);
    void Draw(SpriteBatch spriteBatch, PingPongContext context);
}