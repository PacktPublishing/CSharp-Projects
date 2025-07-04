using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PingPongMono.Entities;

public class Ball(int x, int y, int size, int speed) : PingPongEntity(new Rectangle(x,y, size, size))
{
    public bool IsFacingLeft => _velocity.X < 0;
    public bool IsFacingRight => _velocity.X > 0;
    public int Size => size;

    private Vector2 _velocity = new(speed, speed);

    public override void Update(PingPongContext context)
    {
        base.Update(context);
        
        float factor = context.DeltaTime * 60;
        Bounds = Bounds with
        {
            X = Bounds.X + (int)(_velocity.X * factor),
            Y = Bounds.Y + (int)(_velocity.Y * factor)
        };
    }

    public void FlipHorizontal() => _velocity.X *= -1;
    public void FlipVertical() => _velocity.Y *= -1;

    public void Reset(PingPongContext context)
    {
        Bounds = Bounds with
        {
            X = context.Width / 2 - size / 2,
            Y = context.Height / 2 - size / 2
        };

        bool isGoingLeft = Random.Shared.Next(2) == 0;
        bool isGoingUp = Random.Shared.Next(2) == 0;

        _velocity = new Vector2(
            speed * (isGoingLeft ? -1 : 1),
            speed * (isGoingUp ? -1 : 1));
    }

    public override void Draw(SpriteBatch spriteBatch, PingPongContext context)
    {
        spriteBatch.Draw(context.WhitePixel, Bounds, Color.White);
    }
}