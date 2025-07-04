using PingPongMono.Entities;

namespace PingPongMono.Systems;

public class PaddleCollisionSystem(IPingPongEntity left, IPingPongEntity right, Ball ball) : IPingPongSystem
{
    public void Update(PingPongContext context)
    {
        if ((ball.Bounds.Intersects(left.Bounds) && ball.IsFacingLeft) || 
            (ball.Bounds.Intersects(right.Bounds) && ball.IsFacingRight))
        {
            ball.FlipHorizontal();
        }
        
        if (ball.Bounds.Y <= 0 || ball.Bounds.Y >= context.Height - ball.Size)
        {
            ball.FlipVertical();
        }
    }
}