using ConsoleRolePlayingGame.Domain;
using Spectre.Console;

namespace ConsoleRolePlayingGame.ConsoleApp.Screens;

public class ScreenManager(GameManager game, IAnsiConsole console, OverworldScreen overworld, BattleScreen battleScreen)
{
    public async Task RunAsync()
    {
        // Ensures the app is rendered at a consistent size
        console.Clear();
        
        switch (game.Status)
        {
            case GameStatus.Overworld:
                console.Write(overworld.GenerateVisual());
                await overworld.HandlePlayerInputAsync();
                break;
            
            case GameStatus.Combat:
                console.Write(battleScreen.GenerateVisual());
                await battleScreen.HandlePlayerInputAsync();
                break;         
            
            case GameStatus.GameOver:
                console.MarkupLine("[red]Game Over[/]");
                console.MarkupLine("[yellow]Press any key to exit...[/]");
                await console.Input.ReadKeyAsync(intercept: true, CancellationToken.None);
                game.Quit();
                break;
        }
    }
}