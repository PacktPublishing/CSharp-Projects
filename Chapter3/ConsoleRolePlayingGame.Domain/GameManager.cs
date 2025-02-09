using ConsoleRolePlayingGame.Domain.Combat;
using ConsoleRolePlayingGame.Domain.Commands;
using ConsoleRolePlayingGame.Domain.Overworld;

namespace ConsoleRolePlayingGame.Domain;

public class GameManager
{
    public GameStatus Status { get; internal set; } = GameStatus.Overworld;
    public WorldMap Map { get; }
    public CommandRegistry Commands { get; } = new();
    public Party Party { get; } = new();
    public Battle? Battle { get; private set; }

    public GameManager()
    {
        MapGenerator generator = new();
        Map = new WorldMap(generator);
        Map.AddEntity(Party);
        Map.AddEntity(new EnemyGroup(new Pos(7,3))
        {
            Members = [..Enumerable.Range(1, 8).Select(CreateGoblin)]
        });
    }

    private static Enemy CreateGoblin(int index) 
        => new Enemy { 
            Name = $"Goblin {index}", 
            Health = 5, 
            MaxHealth = 5, 
            Strength = 3, 
            Dexterity = 2, 
            Intelligence = 1, 
            Speed = 5, 
            AsciiArt = [
                @"  o   ",
                @" .l-> ",  
                @" /\   ",
            ] 
        };

    public void Update()
    {
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
            if (Party.IsDead)
            {
                TotalPartyWipe();
            } 
            else if (Battle is not null && Battle.Enemies.IsDead)
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