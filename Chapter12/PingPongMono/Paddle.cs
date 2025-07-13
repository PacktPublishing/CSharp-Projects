using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PingPongMono;

public class Paddle(int x, int y, int width, int height, int speed)
{
    public Rectangle Bounds { get; private set; } = new(x, y, width, height);
    public required Keys UpKey { get; init; }
    public required Keys DownKey { get; init; }
    public Color Color { get; init; } = Color.White;


    public void Update(KeyboardState keyboard, int maxY, float deltaTime)
    {
        int moveAmount = (int)(speed * deltaTime * 60);
        if (keyboard.IsKeyDown(UpKey) && Bounds.Top > 0)
        {
            Bounds = Bounds with { Y = Bounds.Y - moveAmount};
        }
        else if (keyboard.IsKeyDown(DownKey) && Bounds.Bottom < maxY)
        {
            Bounds = Bounds with { Y = Bounds.Y + moveAmount};
        }
    }
    
    public void Draw(SpriteBatch spriteBatch, Texture2D texture)
    {
        spriteBatch.Draw(texture, Bounds, Color);
    }
}