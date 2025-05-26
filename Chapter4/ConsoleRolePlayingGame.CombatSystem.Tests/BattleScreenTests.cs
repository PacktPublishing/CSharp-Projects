using ConsoleRolePlayingGame.ConsoleApp.Input;
using ConsoleRolePlayingGame.ConsoleApp.Screens;
using ConsoleRolePlayingGame.Domain;
using ConsoleRolePlayingGame.Domain.Entities;
using ConsoleRolePlayingGame.Domain.Repositories;
using ConsoleRolePlayingGame.Overworld;
using ConsoleRolePlayingGame.Overworld.Generators;
using ConsoleRolePlayingGame.Overworld.Structure;
using Moq;
using Spectre.Console.Rendering;
using Spectre.Console.Testing;
using Shouldly;
using Spectre.Console;

namespace ConsoleRolePlayingGame.CombatSystem.Tests;

public class BattleScreenTests
{
    [Fact]
    public void BattleScreenShouldRenderHeroesAndEnemies()
    {
        // Arrange
        TestConsole console = new();
        GameManager game = CreateGameManager(console);
        game.StartBattle(CreateTestEncounter());
        BattleScreen screen = new(game, console);

        // Act
        IRenderable renderable = screen.GenerateVisual();
        console.Write(renderable);

        // Assert
        console.Output.ShouldContain("The Party");
        console.Output.ShouldContain("Test Encounter");
        console.Output.ShouldContain("Hero");
        console.Output.ShouldContain("Enemy");
    }

    [Fact]
    public async Task BattleScreenShouldWaitForInput()
    {
        // Arrange
        TestConsole console = new();
        GameManager game = CreateGameManager(console);
        game.StartBattle(CreateTestEncounter());
        BattleScreen screen = new(game, console);

        // Act
        await screen.HandlePlayerInputAsync();

        // Assert
        console.Output.ShouldContain("Wait for next combatant...");
    }

    
    [Fact]
    public void AdvanceUntilNextTurnShouldFavorFastestCombatant()
    {
        // Arrange
        TestConsole console = new();
        GameManager game = CreateGameManager(console);
        Battle battle = game.StartBattle(CreateTestEncounter());

        // Act
        battle.AdvanceUntilNextTurn();

        // Assert
        battle.ActiveMember.ShouldBe(battle.Party.Members[0]);
    }
    
    [Fact]
    public async Task AttackingShouldDamageEnemy()
    {
        // Arrange
        TestConsole console = new();
        GameManager game = CreateGameManager(console);
        Battle battle = game.StartBattle(CreateTestEncounter());
        battle.AdvanceUntilNextTurn();

        BattleScreen screen = new(game, console);
        console.Interactive();
        console.Input.PushKey(ConsoleKey.Enter); // Select attack action
        console.Input.PushKey(ConsoleKey.Enter); // Select target
        console.Input.PushKey(ConsoleKey.Enter); // Acknowledge text output

        // Act
        await screen.HandlePlayerInputAsync();

        // Assert
        Combatant enemy = battle.Enemies.Members[0];
        enemy.Health.ShouldBeLessThan(enemy.MaxHealth);
    }

    
    [Fact]
    public async Task HeavyAttackShouldKillEnemyButCostMana()
    {
        // Arrange
        TestConsole console = new();
        GameManager game = CreateGameManager(console);
        Battle battle = game.StartBattle(CreateTestEncounter());
        battle.AdvanceUntilNextTurn();

        BattleScreen screen = new(game, console);
        console.Interactive();
        console.Input.PushKey(ConsoleKey.DownArrow); // Select heavy attack action
        console.Input.PushKey(ConsoleKey.Enter); // Confirm
        console.Input.PushKey(ConsoleKey.Enter); // Select target
        console.Input.PushKey(ConsoleKey.Enter); // Acknowledge text output

        // Act
        await screen.HandlePlayerInputAsync();

        // Assert
        Combatant enemy = battle.Enemies.Members[0];
        enemy.IsDead.ShouldBeTrue();
        battle.Party.Members[0].Mana.ShouldBe(3);
    }    
    
    private static GameManager CreateGameManager(IAnsiConsole console)
    {
        PlayerParty party = CreateTestParty(console);
        WorldMap map = new(new MapGenerator());
        Mock<IEncounterProvider> mockEncounters = new();
        mockEncounters.Setup(m => m.CreateRandomEncounter(It.IsAny<Pos>()))
            .Returns(CreateTestEncounter());

        return new GameManager(party, mockEncounters.Object, map);
    }

    private static EnemyGroup CreateTestEncounter()
    {
        CombatantData data = new()
        {
            Name = "Enemy",
            AsciiArt = ["Foe"],
            MaxHealth = 2,
            MaxMana = 2,
            Dexterity = 1,
            Intelligence = 1,
            Speed = 2,
            Strength = 1
        };

        return new EnemyGroup
        {
            Name = "Test Encounter",
            Members =
            [
                new Combatant(data, new EnemyTurnStrategy())
                {
                    Abilities = [CreateAttackAbility()]
                }
            ],
        };
    }

    private static PlayerParty CreateTestParty(IAnsiConsole console)
    {
        CombatantData data = new()
        {
            Name = "Hero",
            AsciiArt = ["Test"],
            MaxHealth = 10,
            MaxMana = 5,
            Dexterity = 1,
            Intelligence = 1,
            Speed = 3, // This should be higher than the enemy's speed
            Strength = 1,
        };

        return new()
        {
            Members =
            [
                new(data, new PlayerTurnStrategy(console))
                {
                    IsPlayer = true,
                    Abilities = [
                        CreateAttackAbility(), 
                        CreateHeavyAttackAbility()
                    ]
                }
            ]
        };
    }

    private static Ability CreateAttackAbility()
    {
        return new Ability
        {
            Attribute = Trait.Strength,
            Id = "1",
            Name = "Attack",
            IsHeal = false,
            TargetsSingle = true,
            ManaCost = 0,
            MinMultiplier = 1,
            MaxMultiplier = 1,
        };
    }
    
    private static Ability CreateHeavyAttackAbility()
    {
        return new Ability
        {
            Attribute = Trait.Strength,
            Id = "2",
            Name = "Heavy Attack",
            IsHeal = false,
            TargetsSingle = true,
            ManaCost = 2,
            MinMultiplier = 5,
            MaxMultiplier = 5,
        };
    }
}