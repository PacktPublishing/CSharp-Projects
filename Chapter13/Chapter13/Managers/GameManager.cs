using System.Collections.Generic;
using System.Linq;
using Chapter13.Domain;
using Microsoft.Xna.Framework;

namespace Chapter13.Managers;

public class GameManager(Game game)
{
    public GameMap CurrentMap { get; set; } = new() // TODO: Load this from a JSON data file
    {
        Name = "Alpha",
        Locations = [
            new MapLocation {
                Name = "Mining Colony",
                Type = LocationType.SpaceStation,
                Owner = Faction.Neutral,
                StartPosition = new Vector2(140, 90),
                Size = 32f
            },
            new MapLocation {
                Name = "Deep Space Research Station",
                Type = LocationType.SpaceStation,
                Owner = Faction.Neutral,
                StartPosition = new Vector2(550, 370),
                Size = 16f
            },
            new MapLocation {
                Name = "Patrol Base Alpha",
                Type = LocationType.SpaceStation,
                Owner = Faction.Military,
                StartPosition = new Vector2(220, 200),
                Size = 16f
            },
            new MapLocation {
                Name = "The Space Bar",
                Type = LocationType.SpaceStation,
                Owner = Faction.Criminal,
                StartPosition = new Vector2(600, 50),
                Size = 24f
            },
            new MapLocation {
                Name = "TNS Concordia",
                Type = LocationType.CapitalShip,
                Owner = Faction.Military,
                StartPosition = new Vector2(400, 240),
                Size = 16f
            },
            new MapLocation {
                Name = "To Beta",
                Type = LocationType.JumpGate,
                Owner = Faction.Neutral,
                StartPosition = new Vector2(60, 200),
                Size = 8f
            },
            new MapLocation {
                Name = "To Gamma",
                Type = LocationType.JumpGate,
                Owner = Faction.Neutral,
                StartPosition = new Vector2(750, 320),
                Size = 8f
            },
        ]
    };

    private const int MaxShips = 3;
    public int TrackedShipsCount { get; set; }
    public bool CanSpawnMoreShips => TrackedShipsCount < MaxShips;
    public void Exit() => game.Exit();
}