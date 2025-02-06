using ConsoleRolePlayingGame.Domain.Combat;
using ConsoleRolePlayingGame.Domain.Overworld;

namespace ConsoleRolePlayingGame.Domain;

public class GameManager
{
    public GameStatus Status { get; private set; } = GameStatus.Overworld;
    public WorldMap Map { get; }
    public Battle? Battle { get; private set; }
    
    public GameManager()
    {
        MapGenerator generator = new();
        Map = new WorldMap(generator);
    }
}