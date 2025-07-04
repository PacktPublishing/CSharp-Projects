using PingPongMono.Entities;

namespace PingPongMono.Systems;

public class PaddleCollisionSystem(Paddle left, Paddle right, Ball ball) : IPingPongSystem
{
    public void Update(PingPongContext context)
    {
        if ((ball.Bounds.Intersects(left.Bounds) && ball.IsFacingLeft) || 
            (ball.Bounds.Intersects(right.Bounds) && ball.IsFacingRight))
        {
            ball.FlipHorizontal();
        }
    }
}