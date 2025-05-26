using ConsoleRolePlayingGame.CombatSystem;
using ConsoleRolePlayingGame.ConsoleApp.Screens;
using ConsoleRolePlayingGame.Domain;
using ConsoleRolePlayingGame.Domain.Entities;
using ConsoleRolePlayingGame.Domain.Repositories;
using ConsoleRolePlayingGame.Overworld.Generators;
using ConsoleRolePlayingGame.Overworld.Structure;
using Moq;
using Shouldly;
using Spectre.Console.Rendering;
using Spectre.Console.Testing;

namespace ConsoleRolePlayingGame.Overworld.Tests;

public class OverworldScreenTests
{
    [Fact]
    public void OverworldScreenShouldRenderInfo()
    {
        // Arrange
        PlayerParty party = CreateTestParty();
        WorldMap map = new(new MapGenerator());
        Mock<IEncounterProvider> mockEncounters = new();
        mockEncounters.Setup(m => m.CreateRandomEncounter(It.IsAny<Pos>()))
            .Returns(CreateTestEncounter());
        GameManager game = new(party, mockEncounters.Object, map);
        TestConsole console = new();
        OverworldScreen screen = new(game, console);

        // Act
        IRenderable visual = screen.GenerateVisual();
        console.Write(visual);

        // Assert
        console.ShouldNotBeNull();
        console.Lines.Count.ShouldBeGreaterThan(10);
        console.Lines[0].ShouldStartWith("Console Role Playing Game");
        console.Output.ShouldContain("Test Hero");
        console.Output.ShouldContain("HP  ██████████████████████ 10");
    }

    private static EnemyGroup CreateTestEncounter()
    {
        return new EnemyGroup
        {
            Name = "Test Encounter",
            MapPos = new Pos(5, 5),
            Members = [],
        };
    }

    private static PlayerParty CreateTestParty()
    {
        return new()
        {
            Members = [CreateTestCombatant()]
        };
    }

    private static Combatant CreateTestCombatant()
    {
        return new(new CombatantData
        {
            Name = "Test Hero",
            AsciiArt = ["Test"],
            MaxHealth = 10,
            MaxMana = 5
        }, new EnemyTurnStrategy());
    }
}