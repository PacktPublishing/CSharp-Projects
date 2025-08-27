using System;
using System.Collections.Generic;
using Chapter13.Components;
using Chapter13.Domain;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;

namespace Chapter13.Helpers;

public static class ShipConfigurationHelper
{
    private static readonly IDictionary<ShipType, ShipCharacteristics> _shipCharacteristics = new Dictionary<ShipType, ShipCharacteristics>()
    {
        { ShipType.Freighter, new ShipCharacteristics(12, 10, 10) },
        { ShipType.Patrol, new ShipCharacteristics(4, 30, 35) },
        { ShipType.Raider, new ShipCharacteristics(6, 22, 28) },
    };
    
    public static void ConfigureShip(this Entity ship, Faction faction, ShipType shipType, Vector2 position, Vector2? targetLocation = null)
    {
        ShipCharacteristics stats = _shipCharacteristics[shipType];
        
        ship.Attach(new Transform2(position.X, position.Y)
        {
            Scale = new Vector2(stats.Size),
            Rotation = GetRandomHeadingInRadians()
        });
        ship.Attach(new EngineComponent
        {
            MaxSpeed = stats.MaxSpeed,
            MaxTurnRate = stats.TurnRate
        });
        ship.Attach(new NavigationDataComponent
        {
            TargetLocation = targetLocation
        });
    }

    private static float GetRandomHeadingInRadians()
    {
        return MathHelper.ToRadians(Random.Shared.Next(360));
    }
}