using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PingPongMono.Entities;

public class Ball(int x, int y) : IPingPongEntity
{
    private const int Size = 10;
    private const int Speed = 4;
    
    public Rectangle Bounds { get; private set; } = new(x, y, Size, Size);
    public bool IsFacingLeft => _velocity.X < 0;
    public bool IsFacingRight => _velocity.X > 0;
    
    private Vector2 _velocity = new(Speed, Speed);

    public void Update(PingPongContext context)
    {
        float factor = context.DeltaTime * 60;
        Bounds = Bounds with
        {
            X = Bounds.X + (int)(_velocity.X * factor),
            Y = Bounds.Y + (int)(_velocity.Y * factor)
        };

        if (Bounds.Y <= 0 || Bounds.Y >= context.Height - Size)
        {
            FlipVertical();
        }
    }

    public void FlipHorizontal() => _velocity.X *= -1;
    public void FlipVertical() => _velocity.Y *= -1;

    public void Reset(PingPongContext context)
    {
        Bounds = Bounds with
        {
            X = context.Width / 2 - Size / 2,
            Y = context.Height / 2 - Size / 2
        };

        bool isGoingLeft = Random.Shared.Next(2) == 0;
        bool isGoingUp = Random.Shared.Next(2) == 0;

        _velocity = new Vector2(
            Speed * (isGoingLeft ? -1 : 1),
            Speed * (isGoingUp ? -1 : 1));
    }

    public void Draw(SpriteBatch spriteBatch, PingPongContext context)
    {
        spriteBatch.Draw(context.WhitePixel, Bounds, Color.White);
    }
}