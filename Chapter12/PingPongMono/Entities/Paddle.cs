using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PingPongMono.Entities;

public class Paddle(int x, int y, int width, int height) : IPingPongEntity
{
    private const int PaddleSpeed = 5;
    public Rectangle Bounds { get; private set; } = new(x, y, width, height);
    public Color Color { get; init; } = Color.White;
    public required Keys UpKey { get; init; }
    public required Keys DownKey { get; init; }

    public void Update(PingPongContext context)
    {
        int moveAmount = (int)(PaddleSpeed * context.DeltaTime * 60);
        if (context.Keys.IsKeyDown(UpKey) && Bounds.Y > 0)
        {
            Bounds = new Rectangle(Bounds.X, Bounds.Y - moveAmount, Bounds.Width, Bounds.Height);
        }
        else if (context.Keys.IsKeyDown(DownKey) && Bounds.Y < context.Height - Bounds.Height)
        {
            Bounds = new Rectangle(Bounds.X, Bounds.Y + moveAmount, Bounds.Width, Bounds.Height);
        }
    }
    
    public void Draw(SpriteBatch spriteBatch, PingPongContext context)
    {
        spriteBatch.Draw(context.WhitePixel, Bounds, Color);
    }
}