using System.Collections.Generic;
using System.Linq;
using Chapter13.Domain;
using Microsoft.Xna.Framework;

namespace Chapter13.Managers;

public class GameManager
{
    private readonly Game _game;

    public GameManager(Game game)
    {
        _game = game;
        CurrentMap = new() // TODO: Load this from a JSON data file
    {
        Name = "Alpha",
        Locations = [
            new MapLocation {
                Id = 1,
                Name = "Mining Colony",
                Type = LocationType.SpaceStation,
                Owner = Faction.Neutral,
                StartPosition = new Vector2(140, 90),
                Size = 20f,
                DefaultTargetLocationId = 7
            },
            new MapLocation {
                Id = 2,
                Name = "Deep Space Research Station",
                Type = LocationType.SpaceStation,
                Owner = Faction.Neutral,
                StartPosition = new Vector2(550, 370),
                Size = 16f,
                DefaultTargetLocationId = 1
            },
            new MapLocation {
                Id = 3,
                Name = "Patrol Base Alpha",
                Type = LocationType.SpaceStation,
                Owner = Faction.Military,
                StartPosition = new Vector2(220, 200),
                Size = 16f,
                DefaultTargetLocationId = 5
            },
            new MapLocation {
                Id = 4,
                Name = "The Space Bar",
                Type = LocationType.SpaceStation,
                Owner = Faction.Criminal,
                StartPosition = new Vector2(600, 50),
                Size = 24f,
                DefaultTargetLocationId = 2
            },
            new MapLocation {
                Id = 5,
                Name = "TNS Concordia",
                Type = LocationType.CapitalShip,
                Owner = Faction.Military,
                StartPosition = new Vector2(400, 240),
                Size = 16f,
                DefaultTargetLocationId = 3
            },
            new MapLocation {
                Id = 6,
                Name = "To Beta",
                Type = LocationType.JumpGate,
                Owner = Faction.Neutral,
                StartPosition = new Vector2(60, 200),
                Size = 10f,
                DefaultTargetLocationId = 1
            },
            new MapLocation {
                Id = 7,
                Name = "To Gamma",
                Type = LocationType.JumpGate,
                Owner = Faction.Neutral,
                StartPosition = new Vector2(750, 320),
                Size = 10f,
                DefaultTargetLocationId = 2
            },
        ]
    };
    }

    public GameMap CurrentMap { get; set; }

    private const int MaxShips = 15;
    public int TrackedShipsCount { get; set; }
    public bool CanSpawnMoreShips => TrackedShipsCount < MaxShips;
    public void Exit() => _game.Exit();
}