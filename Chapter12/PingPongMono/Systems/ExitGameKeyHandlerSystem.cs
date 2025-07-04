using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PingPongMono.Systems;

public class ExitGameKeyHandlerSystem(Game game) : IPingPongSystem, IDrawable
{
    private const string Message = "Press ESC to exit";

    public void Update(PingPongContext context)
    {
        if (context.Keys.IsKeyDown(Keys.Escape))
        {
            game.Exit();
        }
    }

    public void Draw(SpriteBatch spriteBatch, PingPongContext context)
    {
        SpriteFont font = context.SmallFont;
        Vector2 pos = new(10, 10);
        spriteBatch.DrawString(font, Message, pos, Color.White);
    }
}