using System;
using System.Linq;
using Chapter13.Components;
using Chapter13.Helpers;
using Chapter13.Managers;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

namespace Chapter13.Systems;

public class WeaponsSystem(GameManager game, SpriteManager sprites) 
    : EntityUpdateSystem(Aspect.All(typeof(HullComponent), typeof(Transform2)))
{
    private ComponentMapper<LauncherComponent> _launcherMapper;
    private ComponentMapper<Transform2> _transformMapper;
    private Random _random;
    private ComponentMapper<HullComponent> _hullMapper;

    public override void Initialize(IComponentMapperService mapperService)
    {
        _launcherMapper = mapperService.GetMapper<LauncherComponent>();
        _hullMapper = mapperService.GetMapper<HullComponent>();
        _transformMapper = mapperService.GetMapper<Transform2>();
        _random = Random.Shared;
    }
    
    public override void Update(GameTime gameTime)
    {
        bool canSpawn = true;
        
        foreach (var entityId in ActiveEntities.OrderBy(_ => _random.Next()))
        {
            // Ignore entities without a launcher component
            if (!_launcherMapper.Has(entityId)) continue;
            
            LauncherComponent launcher = _launcherMapper.Get(entityId);

            launcher.Update(gameTime);
            if (!launcher.IsReadyToSpawn) continue;
            
            Transform2 transform = _transformMapper.Get(entityId);
            
            Vector2 forward = new((float)Math.Cos(transform.Rotation), (float)Math.Sin(transform.Rotation));
            Vector2 targetPos = transform.Position + forward * launcher.DetectionDistance;
            CircleF detectionCircle = new(targetPos, launcher.DetectionRadius);
            
            foreach (var otherEntityId in ActiveEntities)
            {
                // Ignore self
                if (otherEntityId == entityId) continue;
                
                Transform2 otherTransform = _transformMapper.Get(otherEntityId);
                
                // Check if the other entity is within the detection circle
                if (detectionCircle.Contains(otherTransform.Position))
                {
                    Entity missile = CreateEntity();
                    missile.ConfigureShip(transform.Position, launcher.ShipType, otherTransform.Position);
                    launcher.ResetSpawnTimer();
                }
            }
        }
    }
}