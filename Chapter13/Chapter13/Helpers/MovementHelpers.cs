using System;
using Microsoft.Xna.Framework;

namespace Chapter13.Helpers;

public static class MovementHelpers
{
    public static float GetRandomHeadingInRadians()
    {
        return MathHelper.ToRadians(Random.Shared.Next(360));
    }
}