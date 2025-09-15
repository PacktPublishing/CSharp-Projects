using Chapter13.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

namespace Chapter13.Systems;

public class CollisionSystem() 
    : EntityUpdateSystem(Aspect.All(typeof(Transform2))
                               .One(typeof(HullComponent), typeof(HangarComponent)))
{
    private ComponentMapper<Transform2> _transformMapper;
    private ComponentMapper<HullComponent> _hullMapper;
    private ComponentMapper<HangarComponent> _hangarMapper;

    public override void Initialize(IComponentMapperService mapperService)
    {
        _transformMapper = mapperService.GetMapper<Transform2>();
        _hullMapper = mapperService.GetMapper<HullComponent>();
        _hangarMapper = mapperService.GetMapper<HangarComponent>();
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var entity in ActiveEntities)
        {
            Transform2 transform = _transformMapper.Get(entity);
            if (!_hullMapper.Has(entity)) continue;
            
            // Ensure we can collide
            HullComponent hull = _hullMapper.Get(entity);
            hull.Update(gameTime);
            if (!hull.IsCollisionEnabled) continue;
            
            CircleF selfBounds = new(transform.Position, transform.Scale.X / 2f);
            
            // Check for collisions with other entities
            foreach (var otherEntity in ActiveEntities)
            {
                if (otherEntity == entity) continue; // Skip self
                
                Transform2 otherTransform = _transformMapper.Get(otherEntity);
                CircleF otherBounds = new(otherTransform.Position, otherTransform.Scale.X / 2f);
                
                if (selfBounds.Intersects(otherBounds))
                {
                    HandleCollision(entity, otherEntity, _hangarMapper.Has(otherEntity));
                }
            }
        }
    }

    private void HandleCollision(int entity, int otherEntity, bool isWithHangar)
    {
        DestroyEntity(entity);

        if (!isWithHangar)
        {
            DestroyEntity(otherEntity);
        }
    }
}