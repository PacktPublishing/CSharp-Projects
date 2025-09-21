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
        Vector2 toTarget = entity.Transform.Position - entity.Target.Transform.Position;
        float distance = toTarget.Length();
        if (distance > MaxLaunchDistance) return false;

        // Determine the angle of the target from the ship's facing
        float angleToTarget = (float)Math.Atan2(toTarget.Y, toTarget.X);
        float relativeAngle = MathHelper.WrapAngle(angleToTarget - entity.Transform.Rotation);
        return relativeAngle <= MaxLaunchAngle;
    }

    public void Execute(SpaceEntityBase entity, GameTime time)
    {
        ShipEntity ship = (ShipEntity)entity;
        ship.TimeUntilReadyToFire = MinSecondsBetweenLaunch;
        game.SpawnMissile(entity.Transform, entity.Target);
    }
}