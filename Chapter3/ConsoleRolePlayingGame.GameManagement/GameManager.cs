using ConsoleRolePlayingGame.Overworld;
using ConsoleRolePlayingGame.Overworld.Generators;
using ConsoleRolePlayingGame.Overworld.Structure;

namespace ConsoleRolePlayingGame.Domain;

public class GameManager
{
    public GameStatus Status { get; private set; } = GameStatus.Overworld;
    public WorldMap Map { get; }
    public PlayerParty Party { get; }
    
    public const int MaxEnemies = 5;

    public GameManager(PlayerParty party, WorldMap map)
    {
        Party = party;
        Map = map;
        Map.AddEntity(Party);
        
        // Spawn a few initial encounters
        for (int i = 0; i < MaxEnemies; i++)
        {
            SpawnNearbyEncounter();
        }
    }

private void SpawnNearbyEncounter()
{
    OpenPosSelector selector = new(Map);
    Pos point = selector.GetOpenPositionNear(Party.MapPos, 5, 10);
    Map.AddEntity(new EnemyGroup(point));
}
    
    public void MoveParty(Direction dir)
    {
        Party.Move(dir);

        List<EnemyGroup> enemies = Map.Entities.OfType<EnemyGroup>()
            .Where(e => e.MapPos == Party.MapPos)
            .ToList();

        foreach (var group in enemies)
        {
            Map.RemoveEntity(group);
        }
    }
    
    public void Quit() => Status = GameStatus.Terminated;

    public void Update()
    {
        if (Status != GameStatus.Overworld) return;
        
        List<EnemyGroup> enemies = Map.Entities.OfType<EnemyGroup>().ToList();
        
        foreach (var group in enemies)
        {
            group.MoveTowards(Party.MapPos, Map);
            if (group.MapPos == Party.MapPos)
            {
                Map.RemoveEntity(group);
                Party.Health--;
            }
        }
        
        if (Party.Health <= 0)
        {
            Status = GameStatus.GameOver;
        }
        
        if (Map.Entities.OfType<EnemyGroup>().Count() < MaxEnemies)
        {
            SpawnNearbyEncounter();
        }
    }
}