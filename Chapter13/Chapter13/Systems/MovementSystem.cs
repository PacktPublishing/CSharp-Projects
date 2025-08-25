using System;
using Chapter13.Components;
using Chapter13.Managers;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

namespace Chapter13.Systems;

public class MovementSystem(GameManager game) : EntityUpdateSystem(Aspect.All(typeof(EngineComponent), typeof(Transform2)))
{
    private ComponentMapper<EngineComponent> _engineMapper;
    private ComponentMapper<Transform2> _transformMapper;

    public override void Initialize(IComponentMapperService mapperService)
    {
        _engineMapper = mapperService.GetMapper<EngineComponent>();
        _transformMapper = mapperService.GetMapper<Transform2>();
    }

    public override void Update(GameTime gameTime)
    {
        game.TrackedShipsCount = ActiveEntities.Count;
        
        foreach (var entityId in ActiveEntities)
        {
            EngineComponent engine = _engineMapper.Get(entityId);
            Transform2 transform = _transformMapper.Get(entityId);

            if (engine.TargetLocation.HasValue)
            {
                // Rotate towards target
                Vector2 toTarget = engine.TargetLocation.Value - transform.Position;
                float desiredAngle = (float)Math.Atan2(toTarget.Y, toTarget.X);
                float angleDifference = MathHelper.WrapAngle(desiredAngle - transform.Rotation);
                float maxTurn = engine.MaxTurnRate * (float)gameTime.ElapsedGameTime.TotalSeconds;
                float turnAmount = MathHelper.Clamp(angleDifference, -maxTurn, maxTurn);
                transform.Rotation += turnAmount;
            }

            float moveAmount = engine.MaxSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move forward along the current rotation
            Vector2 forward = new((float)Math.Cos(transform.Rotation), (float)Math.Sin(transform.Rotation));
            transform.Position += forward * moveAmount;
        }
    }
}