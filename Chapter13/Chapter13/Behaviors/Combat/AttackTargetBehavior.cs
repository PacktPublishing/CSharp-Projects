using Chapter13.Entities;
using Microsoft.Xna.Framework;
using System;

namespace Chapter13.Behaviors.Combat;

public class AttackTargetBehavior(SpaceGame game) : IBehavior
{
    public float MaxLaunchDistance { get; set; } = 250f;
    public float MaxLaunchAngle { get; set; } = 5f;
    public float MinSecondsBetweenLaunch { get; set; } = 3.5f;

    public bool CanExecute(SpaceEntityBase entity, GameTime time)
    {
        // Only ships can attack
        if (entity is not ShipEntity ship) return false;

        // If we don't have a target, we can't launch
        if (entity.Target is null) return false;

        // If we're not reloaded, we can't launch again
        if (ship.TimeUntilReadyToFire > 0) return false;

        // If the target is too far from the ship, we can't launch
        Vector2 toTarget = entity.Target.Transform.Position - entity.Transform.Position;
        float distance = toTarget.Length();
        if (distance > MaxLaunchDistance) return false;

        // Determine the angle between the ship's facing (front) and the direction to the target
        Vector2 shipFront = new Vector2((float)Math.Cos(entity.Transform.Rotation), (float)Math.Sin(entity.Transform.Rotation));
        Vector2 toTargetNormalized = Vector2.Normalize(toTarget);
        float dot = Vector2.Dot(shipFront, toTargetNormalized);
        float angleToTarget = MathF.Acos(Math.Clamp(dot, -1f, 1f)); // Angle in radians

        // Convert MaxLaunchAngle from degrees to radians for comparison
        float maxLaunchAngleRad = MathHelper.ToRadians(MaxLaunchAngle);

        return angleToTarget <= maxLaunchAngleRad;
    }

    public void Execute(SpaceEntityBase entity, GameTime time)
    {
        ShipEntity ship = (ShipEntity)entity;
        ship.TimeUntilReadyToFire = MinSecondsBetweenLaunch;
        game.SpawnMissile(entity, entity.Target);
    }
}