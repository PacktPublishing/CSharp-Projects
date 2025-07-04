using PingPongMono.Entities;

namespace PingPongMono.Components;

public class PaddleAiControlComponent(Ball ball, int speed) : IPingPongComponent
{
    public void Update(IPingPongEntity entity, PingPongContext context)
    {
        int delta = (int)(speed * context.DeltaTime * 60);
        int y = entity.Bounds.Y;
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