using ConsoleRolePlayingGame.Domain.Combat;
using ConsoleRolePlayingGame.Domain.Commands;
using ConsoleRolePlayingGame.Domain.Overworld;
using ConsoleRolePlayingGame.Domain.Repositories;

namespace ConsoleRolePlayingGame.Domain;

public class GameManager
{
    public GameStatus Status { get; internal set; } = GameStatus.Overworld;
    public WorldMap Map { get; }
    public CommandRegistry Commands { get; } = new();
    public Party Party { get; }
    public Battle? Battle { get; private set; }

    public GameManager()
    {
        PartyRepository partyRepository = new();
        Party = partyRepository.Load();
        
        EnemyRepository enemyRepository = new();
        EncounterRepository encounterRepository = new(enemyRepository);
        
        MapGenerator generator = new();
        Map = new WorldMap(generator);
        Map.AddEntity(Party);
        Map.AddEntity(encounterRepository.CreateRandomEncounter(new Pos(7, 3)));
        Map.AddEntity(encounterRepository.CreateRandomEncounter(new Pos(2, -3)));
        Map.AddEntity(encounterRepository.CreateRandomEncounter(new Pos(-1, -8)));
        Map.AddEntity(encounterRepository.CreateRandomEncounter(new Pos(2, 9)));
    }

    public void Update()
    {
        if (Party.IsDead && Status != GameStatus.GameOver)
        {
            TotalPartyWipe();
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

    private void EndBattle()
    {
        Battle = null;
        Status = GameStatus.Overworld;
        
        // TODO: Spawn new enemies to replace the ones that were defeated
    }

    private void TotalPartyWipe()
    {
        Battle = null;
        Status = GameStatus.GameOver;
    }

    private void StartBattle(EnemyGroup enemies)
    {
        Map.RemoveEntity(enemies);
        Battle = new Battle(Party, enemies);
        
        Status = GameStatus.Combat;
    }
}