using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PingPongMono;

public class Ball(int x, int y, int size, int speed)
{
    private Vector2 _velocity = new(speed, speed);
    public Rectangle Bounds { get; private set; } = new(x, y, size, size);
    public bool IsFacingLeft => _velocity.X < 0;
    public bool IsFacingRight => _velocity.X > 0;

    public void Update(int width, int height)
    {
        Bounds = Bounds with
        {
            X = Bounds.X + (int)_velocity.X,
            Y = Bounds.Y + (int)_velocity.Y
        };

        if (Bounds.Y <= 0 || Bounds.Y >= height - size)
        {
            FlipVertical();
        }
    }

    public void FlipHorizontal() => _velocity.X *= -1;
    public void FlipVertical() => _velocity.Y *= -1;

    public void Reset(int width, int height)
    {
        Bounds = Bounds with
        {
            X = width / 2 - size / 2,
            Y = height / 2 - size / 2
        };

        bool isGoingLeft = Random.Shared.Next(2) == 0;
        bool isGoingUp = Random.Shared.Next(2) == 0;

        _velocity = new Vector2(
            speed * (isGoingLeft ? -1 : 1),
            speed * (isGoingUp ? -1 : 1));
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D whiteTexture)
    {
        spriteBatch.Draw(whiteTexture, Bounds, Color.White);
    }
}