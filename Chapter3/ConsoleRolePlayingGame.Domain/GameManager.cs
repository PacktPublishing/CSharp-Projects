using ConsoleRolePlayingGame.Domain.Commands;
using ConsoleRolePlayingGame.Domain.Overworld;

namespace ConsoleRolePlayingGame.Domain;

public class GameManager
{
    public GameStatus Status { get; internal set; } = GameStatus.Overworld;
    public WorldMap Map { get; }
    public CommandRegistry Commands { get; } = new();
    public Party Party { get; } = new();

    public GameManager()
    {
        MapGenerator generator = new();
        Map = new WorldMap(generator);
        Map.AddEntity(Party);
        Map.AddEntity(new EnemyGroup(new Pos(7,3)));
    }

    public void Update()
    {
        if (Status == GameStatus.Overworld)
        {
            if (Map.Entities.OfType<EnemyGroup>().Any(g => g.Position == Party.Position))
            {
                Status = GameStatus.Combat;
            }
        }
        else if (Status == GameStatus.Combat)
        {
            // TODO: Check for party death
            
            // TODO: Check for enemy defeat
            
            // TODO: Spawn new enemies to replace the ones that were defeated
        }
    }
}