using System.Linq;
using Chapter13.Components;
using Chapter13.Domain;
using Chapter13.Helpers;
using Chapter13.Managers;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Graphics;

namespace Chapter13.Systems;

public class MapLoaderSystem(GameManager game, SpriteManager sprites) : UpdateSystem
{
    private GameMap _map;
    private World _world;
    private Entity _legend;
    private TextLabelComponent _systemLabel;

    public override void Initialize(World world)
    {
        _world = world;
        
        _legend = _world.CreateEntity();
        _legend.Attach(new Transform2(10, 10));
        
        _systemLabel = new TextLabelComponent("Loading...")
        {
            GameFont = GameFont.Large
        };
        _legend.Attach(_systemLabel);

        base.Initialize(world);
    }

    public override void Update(GameTime gameTime)
    {
        if (_map == game.CurrentMap) return;
        _map = game.CurrentMap;

        _systemLabel.Text = $"{_map.Name} System";
        
        foreach (var location in _map.Locations)
        {
            Color color = location.Owner.GetFactionColor();
            Entity entity = _world.CreateEntity();
            
            entity.Attach(new Transform2(location.StartPosition.X, location.StartPosition.Y)
            {
                Scale = new Vector2(location.Size),
            });
            entity.Attach(new TextLabelComponent(location.Name)
            {
                Color = color,
                Centered = true,
                GameFont = GameFont.Small
            });
            entity.Attach(new Sprite(sprites.SolidPixelTexture)
            {
                Color = color
            });
            MapLocation target = _map.Locations.FirstOrDefault(l => l.Id == location.DefaultTargetLocationId);
            entity.Attach(new HangarComponent
            {
                Faction = location.Owner,
                ShipType = GetSpawnedShipType(location.Type, location.Owner),
                DefaultTargetLocation = target?.StartPosition,
            });
        }
    }

    private static ShipType GetSpawnedShipType(LocationType locationType, Faction owner)
    {
        return locationType switch
        {
            LocationType.SpaceStation => owner == Faction.Criminal ? ShipType.Raider : ShipType.Freighter,
            LocationType.CapitalShip => ShipType.Patrol,
            _ => ShipType.Freighter
        };
    }
}