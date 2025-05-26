namespace ConsoleRolePlayingGame.ConsoleApp.Screens;

public class ScreenManager(GameManager game, IAnsiConsole console, OverworldScreen overworld)
{
    public void Run()
    {
        // Ensures the app is rendered at a consistent size
        console.Clear();
        
        switch (game.Status)
        {
            case GameStatus.Overworld:
                console.Write(overworld.GenerateVisual());
                overworld.HandlePlayerInput();
                break;
            
            case GameStatus.GameOver:
                console.MarkupLine("[red]Game Over[/]");
                console.MarkupLine("[yellow]Press any key to exit...[/]");
                console.Input.ReadKey(intercept: true);
                game.Quit();
                break;
        }
    }
}