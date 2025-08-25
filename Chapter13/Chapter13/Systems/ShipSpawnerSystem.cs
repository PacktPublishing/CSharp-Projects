using System;
using System.Linq;
using Chapter13.Components;
using Chapter13.Helpers;
using Chapter13.Managers;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Graphics;

namespace Chapter13.Systems;

public class ShipSpawnerSystem(GameManager game, SpriteManager sprites) 
    : EntityUpdateSystem(Aspect.All(typeof(HangarComponent), typeof(Transform2)))
{
    private ComponentMapper<HangarComponent> _hangarMapper;
    private ComponentMapper<Transform2> _transformMapper;
    private Random _random;

    public override void Initialize(IComponentMapperService mapperService)
    {
        _hangarMapper = mapperService.GetMapper<HangarComponent>();
        _transformMapper = mapperService.GetMapper<Transform2>();
        _random = Random.Shared;
    }
    
    public override void Update(GameTime gameTime)
    {
        bool canSpawn = game.CanSpawnMoreShips;
        
        foreach (var entityId in ActiveEntities.OrderBy(_ => _random.Next()))
        {
            HangarComponent hangar = _hangarMapper.Get(entityId);
            hangar.Update(gameTime);
            
            if (canSpawn && hangar.IsReadyToSpawn)
            {
                Transform2 transform = _transformMapper.Get(entityId);
                
                Entity ship = CreateEntity();
                ship.Attach(new Transform2(transform.Position.X, transform.Position.Y)
                {
                    Scale = new Vector2(hangar.ShipSize),
                    Rotation = MathHelper.ToRadians(Random.Shared.Next(360))
                });
                ship.Attach(new EngineComponent()
                {
                    MaxSpeed = 10f,
                    MaxTurnRate = 30f,
                    //TargetLocation = new Vector2(300, 300),
                });
                ship.Attach(new Sprite(sprites.SolidPixelTexture)
                {
                    Color = hangar.Faction.GetFactionColor()
                });
                
                canSpawn = false; // We'll be able to spawn again next update loop
                hangar.ResetSpawnTimer();
            }
        }
    }
}