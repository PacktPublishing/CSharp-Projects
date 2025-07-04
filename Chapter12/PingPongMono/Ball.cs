using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PingPongMono;

public class Ball(int x, int y)
{
    public const int Size = 10;
    public const int Speed = 4;
    public Rectangle Bounds { get; private set; } = new(x, y, Size, Size);
    public bool IsFacingLeft => _velocity.X < 0;
    public bool IsFacingRight => _velocity.X > 0;
    
    private Vector2 _velocity = new(Speed, Speed);

    public void Update(int width, int height, float deltaTime)
    {
        Bounds = Bounds with
        {
            X = Bounds.X + (int)(_velocity.X * deltaTime * 60),
            Y = Bounds.Y + (int)(_velocity.Y * deltaTime * 60)
        };

        if (Bounds.Y <= 0 || Bounds.Y >= height - Size)
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
            X = width / 2 - Size / 2,
            Y = height / 2 - Size / 2
        };

        bool isGoingLeft = Random.Shared.Next(2) == 0;
        bool isGoingUp = Random.Shared.Next(2) == 0;

        _velocity = new Vector2(
            Speed * (isGoingLeft ? -1 : 1),
            Speed * (isGoingUp ? -1 : 1));
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D whiteTexture)
    {
        spriteBatch.Draw(whiteTexture, Bounds, Color.White);
    }
}