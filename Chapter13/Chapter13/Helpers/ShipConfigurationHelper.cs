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
        { ShipType.Freighter, new ShipCharacteristics(8, 10, 10) },
        { ShipType.Patrol, new ShipCharacteristics(4, 30, 35) },
        { ShipType.Raider, new ShipCharacteristics(6, 22, 28) },
    };
    
    public static void ConfigureShip(this Entity ship, Vector2 position, ShipType shipType, Vector2? targetLocation = null)
    {
        ShipCharacteristics stats = _shipCharacteristics[shipType];

        Transform2 shipPos = new(position.X, position.Y)
        {
            Scale = new Vector2(stats.Size),
            Rotation = MovementHelpers.GetRandomHeadingInRadians()
        };
        ship.Attach(shipPos);
        ship.Attach(new EngineComponent
        {
            MaxSpeed = stats.MaxSpeed,
            MaxTurnRate = stats.TurnRate
        });
        ship.Attach(new HullComponent());
        ship.Attach(new NavigationDataComponent
        {
            TargetLocation = targetLocation
        });
        
        if (shipType is ShipType.Patrol or ShipType.Raider)
        {
            ship.Attach(new LauncherComponent());
        }
    }
}