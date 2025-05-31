using ConsoleRolePlayingGame.ConsoleApp.Screens;
using ConsoleRolePlayingGame.Domain;
using ConsoleRolePlayingGame.Overworld.Generators;
using ConsoleRolePlayingGame.Overworld.Structure;
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
        GameManager game = CreateGameManager();
        TestConsole console = new();
        OverworldScreen screen = new(game, console);

        // Act
        IRenderable visual = screen.GenerateVisual();
        console.Write(visual);

        // Assert
        console.ShouldNotBeNull();
        console.Lines.Count.ShouldBeGreaterThan(10);
        console.Lines[0].ShouldStartWith("Console Role Playing Game");
        console.Output.ShouldContain("Hero");
        console.Output.ShouldContain("HP");
        console.Output.ShouldContain("10");
    }

    [Theory]
    [InlineData(ConsoleKey.UpArrow, 0, -1)]
    [InlineData(ConsoleKey.S, 0, 1)]
    [InlineData(ConsoleKey.A, -1, 0)]
    [InlineData(ConsoleKey.RightArrow, 1, 0)]
    public void OverworldScreenShouldHandleMovementCommands(ConsoleKey key, int expectedX, int expectedY)
    {
        // Arrange
        GameManager game = CreateGameManager();
        TestConsole console = new();
        console.Interactive();
        console.Input.PushKey(key);
        OverworldScreen screen = new(game, console);
        
        // Act
        screen.HandlePlayerInput();

        // Assert
        game.Party.MapPos.ShouldBe(new Pos(expectedX, expectedY));
        game.Status.ShouldBe(GameStatus.Overworld);
    }
    
    [Fact]
    public void OverworldScreenShouldHandleQuitKey()
    {
        // Arrange
        GameManager game = CreateGameManager();
        TestConsole console = new();
        console.Interactive();
        console.Input.PushKey(ConsoleKey.Q);
        OverworldScreen screen = new(game, console);
        
        // Act
        screen.HandlePlayerInput();

        // Assert
        game.Status.ShouldBe(GameStatus.Terminated);
    }

    private static GameManager CreateGameManager()
    {
        PlayerParty party = new();
        WorldMap map = new(new MapGenerator());
        return new GameManager(party, map);
    }

}