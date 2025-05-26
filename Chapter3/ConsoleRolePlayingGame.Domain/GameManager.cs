using ConsoleRolePlayingGame.Domain.Entities;
using ConsoleRolePlayingGame.Overworld;
using ConsoleRolePlayingGame.Overworld.Generators;
using ConsoleRolePlayingGame.Overworld.Structure;

namespace ConsoleRolePlayingGame.Domain;

public class GameManager
{
    public GameStatus Status { get; private set; } = GameStatus.Overworld;
    public WorldMap Map { get; }
    public PlayerParty Party { get; }

    public GameManager(PlayerParty party, WorldMap map)
    {
        Party = party;
        Map = map;
        Map.AddEntity(Party);
        
        // Spawn a few initial encounters
        for (int i = 0; i < 5; i++)
        {
            SpawnNearbyEncounter();
        }
    }

    private void SpawnNearbyEncounter()
    {
        OpenPosSelector selector = new(Map, Random.Shared);
        Pos point = selector.GetOpenPositionNear(Party.MapPos, 5, 10);
        Map.AddEntity(new EnemyGroup(point));
    }
    
    public void Quit() => Status = GameStatus.Terminated;
}