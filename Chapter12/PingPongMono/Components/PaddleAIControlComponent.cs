using System;
using System.Collections.Generic;
using System.Linq;
using PingPongMono.Entities;

namespace PingPongMono.Components;

public class PaddleAiControlComponent(int speed) : IPingPongComponent
{
    public void Update(IPingPongEntity entity, PingPongContext context)
    {
        int x = entity.Bounds.X;
        int y = entity.Bounds.Y;

        // Find the closest ball to the paddle
        IEnumerable<Ball> balls = context.World.FindEntities<Ball>();
        Ball? ball = balls.OrderBy(b => Math.Abs(b.Bounds.X - x)).FirstOrDefault();
        if (ball is null) return;

        
        int delta = (int)(speed * context.DeltaTime * 60);
        int yMidpoint = entity.Bounds.Y + entity.Bounds.Height / 2;
        
        if (yMidpoint < ball.Bounds.Top && y < context.Height - entity.Bounds.Height)
        {
            y += delta;
        }
        else if (yMidpoint > ball.Bounds.Bottom && y > 0)
        {
            y -= delta;
        }

        entity.Bounds = entity.Bounds with { Y = y };
    }
}