using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PingPongMono;

public class Ball
{
    public Rectangle Bounds { get; private set; }
    public bool IsFacingLeft => _velocity.X < 0;
    public bool IsFacingRight => _velocity.X > 0;
    
    private Vector2 _velocity;
    private readonly int _size;
    private readonly int _speed;

    public Ball(int width, int height, int size, int speed)
    {
        _size = size;
        _speed = speed;
        Bounds = new Rectangle(0, 0, size, size);
        Reset(width, height);
    }

    public void Update(int height, float deltaTime)
    {
        Bounds = Bounds with
        {
            X = Bounds.X + (int)(_velocity.X * deltaTime * 60),
            Y = Bounds.Y + (int)(_velocity.Y * deltaTime * 60)
        };

        if (Bounds.Top <= 0 || Bounds.Bottom >= height)
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
            X = width / 2 - _size / 2,
            Y = height / 2 - _size / 2
        };

        float xFraction = 0.25f + (float)Random.Shared.NextDouble() * 0.75f;
        float yFraction = (float)Math.Sqrt(1 - xFraction * xFraction);

        int xSign = Random.Shared.Next(2) == 0 ? 1 : -1;
        int ySign = Random.Shared.Next(2) == 0 ? 1 : -1;

        _velocity = new Vector2(
            xSign * xFraction * _speed,
            ySign * yFraction * _speed);
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D whiteTexture)
    {
        spriteBatch.Draw(whiteTexture, Bounds, Color.White);
    }
}