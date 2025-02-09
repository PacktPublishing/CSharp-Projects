using ConsoleRolePlayingGame.ConsoleApp.Screens;
using ConsoleRolePlayingGame.Domain;
using ConsoleRolePlayingGame.Domain.Commands;
using Spectre.Console;

try
{
    GameManager game = new();
    OverworldScreen overworldScreen = new(game);
    BattleScreen battleScreen = new(game);

    while (game.Status != GameStatus.Terminated)
    {
        // Ensures the app is rendered at a consistent size
        AnsiConsole.Clear();

        switch (game.Status)
        {
            case GameStatus.Overworld:
                AnsiConsole.Write(overworldScreen.GenerateVisual());
                break;
            
            case GameStatus.Combat:
                AnsiConsole.Write(battleScreen.GenerateVisual());
                break;         
            
            case GameStatus.GameOver:
                AnsiConsole.MarkupLine("[red]Game Over[/]");
                break;
        }

        // Get input from the user
        // TODO: This is probably only needed on the overworld
        AnsiConsole.Markup("[Yellow]>[/] ");
        ConsoleKeyInfo? keyInfo = AnsiConsole.Console.Input.ReadKey(intercept: true);

        if (keyInfo.HasValue)
        {
            // TODO: Commands should be specific to the current game state
            Command? command = game.Commands.GetCommandForKey(keyInfo.Value.Key);
            command?.Execute(game);
        }

        game.Update();
    }
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
    Console.ReadKey();
}