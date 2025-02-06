using ConsoleRolePlayingGame.ConsoleApp;
using ConsoleRolePlayingGame.Domain;
using ConsoleRolePlayingGame.Domain.Commands;
using ConsoleRolePlayingGame.Domain.Overworld;
using Spectre.Console;

try
{
    AnsiConsole.Clear();
    
    GameManager game = new();
    
    const int headerHeight = 2;
    const int footerHeight = 2;
    CanvasMapRenderer renderer = new(AnsiConsole.Profile.Width, AnsiConsole.Profile.Height - headerHeight - footerHeight);
    
    WorldMap map = game.Map;

    do
    {
        DisplayHelpers.WriteHeader(game);
        renderer.Render(game);

        AnsiConsole.WriteLine();
        AnsiConsole.Markup("[Yellow]>[/] ");

        ConsoleKeyInfo? keyInfo = AnsiConsole.Console.Input.ReadKey(intercept: true);

        AnsiConsole.WriteLine();
        AnsiConsole.Clear();

        if (keyInfo.HasValue)
        {
            Command? command = game.Commands.GetCommandForKey(keyInfo.Value.Key);
            command?.Execute(game);
        }
    } while (game.Status != GameStatus.Terminated);
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
}