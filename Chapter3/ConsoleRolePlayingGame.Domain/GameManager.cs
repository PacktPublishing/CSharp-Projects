using ConsoleRolePlayingGame.Domain.Combat;
using ConsoleRolePlayingGame.Domain.Commands;
using ConsoleRolePlayingGame.Domain.Overworld;
using ConsoleRolePlayingGame.Domain.Repositories;

namespace ConsoleRolePlayingGame.Domain;

public class GameManager
{
    private readonly EncounterRepository _encounters;
    public GameStatus Status { get; internal set; } = GameStatus.Overworld;
    public WorldMap Map { get; }
    public CommandRegistry Commands { get; } = new();
    public Party Party { get; }
    public Battle? Battle { get; private set; }

    public GameManager()
    {
        // TODO: DI container would be good here
        PartyRepository partyRepository = new();
        Party = partyRepository.Load();
        
        EnemyRepository enemyRepository = new();
        _encounters = new(enemyRepository);
        
        MapGenerator generator = new();
        Map = new WorldMap(generator);
        Map.AddEntity(Party);
        
        for (int i = 0; i < 5; i++)
        {
            SpawnNearbyEncounter();
        }
    }

    private void SpawnNearbyEncounter()
    {
        Pos point = Map.GetOpenPositionNear(Party.Position, 5, 15);
        Map.AddEntity(_encounters.CreateRandomEncounter(point));
    }

    public void Update()
    {
        if (Party.IsDead && Status != GameStatus.GameOver)
        {
            TriggerGameOver();
            return;
        } 
        
        if (Status == GameStatus.Overworld)
        {
            EnemyGroup? encounter = Map.Entities.OfType<EnemyGroup>()
                .FirstOrDefault(g => g.Position == Party.Position);
            
            if (encounter is not null)
            {
                StartBattle(encounter);
            }
        }
        else if (Status == GameStatus.Combat)
        {
            if (Battle is not null && Battle.Enemies.IsDead)
            {
                EndBattle();
            }
        }
    }

    private void StartBattle(EnemyGroup enemies)
    {
        Map.RemoveEntity(enemies);
        Battle = new Battle(Party, enemies);
        Status = GameStatus.Combat;
    }
    
    private void EndBattle()
    {
        Battle = null;
        Status = GameStatus.Overworld;
        SpawnNearbyEncounter();
    }

    private void TriggerGameOver()
    {
        Battle = null;
        Status = GameStatus.GameOver;
    }
}