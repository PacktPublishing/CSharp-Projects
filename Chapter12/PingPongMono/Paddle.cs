using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PingPongMono;

public class Paddle(int x, int y, int width, int height)
{
    private const int PaddleSpeed = 5;
    public Rectangle Bounds { get; private set; } = new(x, y, width, height);
    public Color Color { get; init; } = Color.White;
    public required Keys UpKey { get; init; }
    public required Keys DownKey { get; init; }

    public void Update(KeyboardState keyboard, int maxY, float deltaTime)
    {
        int moveAmount = (int)(PaddleSpeed * deltaTime * 60);
        if (keyboard.IsKeyDown(UpKey) && Bounds.Y > 0)
        {
            Bounds = new Rectangle(Bounds.X, Bounds.Y - moveAmount, Bounds.Width, Bounds.Height);
        }
        else if (keyboard.IsKeyDown(DownKey) && Bounds.Y < maxY - Bounds.Height)
        {
            Bounds = new Rectangle(Bounds.X, Bounds.Y + moveAmount, Bounds.Width, Bounds.Height);
        }
    }
    
    public void Draw(SpriteBatch spriteBatch, Texture2D texture)
    {
        spriteBatch.Draw(texture, Bounds, Color);
    }
}