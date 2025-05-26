using ConsoleRolePlayingGame.CombatSystem;
using ConsoleRolePlayingGame.Domain.Entities;
using ConsoleRolePlayingGame.Domain.Repositories;
using ConsoleRolePlayingGame.Overworld;
using ConsoleRolePlayingGame.Overworld.Entities;
using ConsoleRolePlayingGame.Overworld.Generators;
using ConsoleRolePlayingGame.Overworld.Structure;

namespace ConsoleRolePlayingGame.Domain;

public class GameManager
{
    private readonly IEncounterProvider _encounters;
    public GameStatus Status { get; private set; } = GameStatus.Overworld;
    public WorldMap Map { get; }
    public PlayerParty Party { get; }
    public Battle? Battle { get; private set; }

    public GameManager(PlayerParty party,
                       IEncounterProvider encounters,
                       WorldMap map)
    {
        _encounters = encounters;
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
        Map.AddEntity(_encounters.CreateRandomEncounter(point));
    }

    public void Update()
    {
        if (Status is GameStatus.Terminated or GameStatus.GameOver)
        {
            return;
        }
        
        if (Party.Members.All(m => m.IsDead))
        {
            TriggerGameOver();
            return;
        } 
        
        switch (Status)
        {
            case GameStatus.Overworld:
                IMapEntity? encounter = Map.Entities
                    .FirstOrDefault(g => g.EntityType == EntityType.Enemy && g.MapPos == Party.MapPos);

                if (encounter is ICombatGroup combatant)
                {
                    Map.RemoveEntity(encounter);
                    StartBattle(combatant);
                }
                break;
            
            case GameStatus.Combat:
                if (Battle is not null && Battle.Enemies.Members.All(e => e.IsDead))
                {
                    EndBattle();
                }
                break;
        }
    }

    public Battle StartBattle(ICombatGroup combatant)
    {
        Battle = new Battle(Party, combatant);
        Status = GameStatus.Combat;
        return Battle;
    }

    public void EndBattle()
    {
        Battle = null;
        Status = GameStatus.Overworld;
        SpawnNearbyEncounter();
    }

    public void Quit() => Status = GameStatus.Terminated;

    private void TriggerGameOver() => Status = GameStatus.GameOver;
}