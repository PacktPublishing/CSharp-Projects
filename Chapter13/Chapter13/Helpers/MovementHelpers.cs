using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Chapter13.Helpers;

public static class MovementHelpers
{
    public static float GetRandomHeadingInRadians()
    {
        return MathHelper.ToRadians(Random.Shared.Next(360));
    }

    public static void RotateTowardsTarget(this Transform2 transform, Vector2 targetPos, float turnRate, GameTime time)
    {
        float angle = CalculateSteeringAngle(transform, targetPos, turnRate, time);
        transform.Rotation += angle;
    }
    public static void RotateAwayFromTarget(this Transform2 transform, Vector2 targetPos, float turnRate, GameTime time)
    {
        float angle = CalculateSteeringAngle(transform, targetPos, turnRate, time);
        transform.Rotation -= angle;
    }

    private static float CalculateSteeringAngle(Transform2 transform, Vector2 targetPos, float turnRate, GameTime time)
    {
        Vector2 position = transform.Position;
        float desiredAngle = (float)Math.Atan2(targetPos.Y - position.Y, targetPos.X - transform.Position.X);
        float angleDifference = MathHelper.WrapAngle(desiredAngle - transform.Rotation);
        float maxTurn = turnRate * (float)time.ElapsedGameTime.TotalSeconds;
        angleDifference = MathHelper.Clamp(angleDifference, -maxTurn, maxTurn);
        return angleDifference;
    }

}