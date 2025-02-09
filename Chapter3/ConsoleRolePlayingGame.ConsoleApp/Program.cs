using ConsoleRolePlayingGame.ConsoleApp.Screens;
using ConsoleRolePlayingGame.ConsoleApp.Visuals;
using ConsoleRolePlayingGame.Domain;
using ConsoleRolePlayingGame.Domain.Commands;
using Spectre.Console;

try
{
    GameManager game = new();
    OverworldScreen overworldScreen = new(game);

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
                throw new NotImplementedException();
                break;
        }

        // Get input from the user
        AnsiConsole.Markup("[Yellow]>[/] ");
        ConsoleKeyInfo? keyInfo = AnsiConsole.Console.Input.ReadKey(intercept: true);

        if (keyInfo.HasValue)
        {
            Command? command = game.Commands.GetCommandForKey(keyInfo.Value.Key);
            command?.Execute(game);
        }
    }
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
}