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
    private ComponentMapper<NavigationDataComponent> _navMapper;

    public override void Initialize(IComponentMapperService mapperService)
    {
        _engineMapper = mapperService.GetMapper<EngineComponent>();
        _transformMapper = mapperService.GetMapper<Transform2>();
        _navMapper = mapperService.GetMapper<NavigationDataComponent>();
    }

    public override void Update(GameTime gameTime)
    {
        //game.TrackedShipsCount = ActiveEntities.Count;

        foreach (int entityId in ActiveEntities)
        {
            EngineComponent engine = _engineMapper.Get(entityId);
            Transform2 transform = _transformMapper.Get(entityId);
            NavigationDataComponent? navData = null;
            if (_navMapper.Has(entityId))
            {
                navData = _navMapper.Get(entityId);
            }

            if (navData != null)
            {
                UpdateTargetLocation(engine, navData);
            }
            RotateTowardsTarget(gameTime, engine, transform);
            MoveForwards(gameTime, engine, transform);
        }
    }

    private static void MoveForwards(GameTime gameTime, EngineComponent engine, Transform2 transform)
    {
        float moveAmount = engine.MaxSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 forward = new((float)Math.Cos(transform.Rotation), (float)Math.Sin(transform.Rotation));
        transform.Position += forward * moveAmount;
    }

    private static void RotateTowardsTarget(GameTime gameTime, EngineComponent engine, Transform2 transform)
    {
        if (!engine.TargetLocation.HasValue) return;

        Vector2 toTarget = engine.TargetLocation.Value - transform.Position;
        float desiredAngle = (float)Math.Atan2(toTarget.Y, toTarget.X);
        float angleDifference = MathHelper.WrapAngle(desiredAngle - transform.Rotation);
        float maxTurn = engine.MaxTurnRate * (float)gameTime.ElapsedGameTime.TotalSeconds;
        float turnAmount = MathHelper.Clamp(angleDifference, -maxTurn, maxTurn);
        transform.Rotation += turnAmount;
    }

    private void UpdateTargetLocation(EngineComponent engine, NavigationDataComponent navData)
    {
        if (navData.TargetEntityId.HasValue)
        {
            Transform2 targetTransform = _transformMapper.Get(navData.TargetEntityId.Value);
            engine.TargetLocation = targetTransform.Position;
        }
        else if (navData.TargetLocation.HasValue)
        {
            engine.TargetLocation = navData.TargetLocation;
        }
    }
}