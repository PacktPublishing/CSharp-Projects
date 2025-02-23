using ConsoleRolePlayingGame.Domain.Combat;
using ConsoleRolePlayingGame.Domain.Overworld;
using ConsoleRolePlayingGame.Domain.Repositories;

namespace ConsoleRolePlayingGame.Domain;

public class GameManager
{
    private readonly EncounterRepository _encounters;
    private readonly Random _random;
    public GameStatus Status { get; private set; } = GameStatus.Overworld;
    public WorldMap Map { get; }
    public CombatGroup Party { get; }
    public Battle? Battle { get; private set; }

    public GameManager(PartyRepository partyRepository, 
                       EncounterRepository encounterRepository,
                       Random random,
                       WorldMap map)
    {
        _encounters = encounterRepository;
        _random = random;
        Party = partyRepository.Load();
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
        Pos point = Map.GetOpenPositionNear(Party.MapPos, 5, 10);
        Map.AddEntity(_encounters.CreateRandomEncounter(point));
    }

    public void Update()
    {
        if (Status is GameStatus.Terminated or GameStatus.GameOver)
        {
            return;
        }
        
        if (Party.IsDead)
        {
            TriggerGameOver();
            return;
        } 
        
        switch (Status)
        {
            case GameStatus.Overworld:
            {
                CombatGroup? encounter = Map.Entities
                    .FirstOrDefault(g => g != Party && g.MapPos == Party.MapPos);
            
                if (encounter is not null)
                {
                    StartBattle(encounter);
                }

                break;
            }
            case GameStatus.Combat:
            {
                if (Battle is not null && Battle.Enemies.IsDead)
                {
                    EndBattle();
                }

                break;
            }
        }
    }

    private void StartBattle(CombatGroup enemies)
    {
        Map.RemoveEntity(enemies);
        Battle = new Battle(Party, enemies, _random);
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

    public void Quit() 
        => Status = GameStatus.Terminated;
}