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
        Random rand = new();
        
        // TODO: Load this map from a file
        Map = new WorldMap(30, 15, rand);
    }
}