using Chapter13.Entities;
using Microsoft.Xna.Framework;
using System;

namespace Chapter13.Behaviors.Combat;

public class AttackTargetBehavior(SpaceGame game) : IBehavior
{
    private GameTime? _lastLaunchTime;

    public float MaxLaunchDistance { get; set; } = 250f;
    public float MaxLaunchAngle { get; set; } = 15f;
    public float MinSecondsBetweenLaunch { get; set; } = 2f;

    public bool CanExecute(SpaceEntityBase entity, GameTime time)
    {
        // If we don't have a target, we can't launch
        if (entity.Target is null) return false;

        // If we're not reloaded, we can't launch again
        if (_lastLaunchTime is not null &&
            (time.TotalGameTime - _lastLaunchTime.TotalGameTime).TotalSeconds < MinSecondsBetweenLaunch)
            return false;

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
        _lastLaunchTime = time;

        game.SpawnMissile(entity.Transform, entity.Target);
    }
}