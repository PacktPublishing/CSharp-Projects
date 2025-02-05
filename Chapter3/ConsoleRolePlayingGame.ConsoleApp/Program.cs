using ConsoleRolePlayingGame.ConsoleApp;
using ConsoleRolePlayingGame.Domain;
using ConsoleRolePlayingGame.Domain.Overworld;
using Spectre.Console;

try
{
    DisplayHelpers.TrySetEncoding();
    DisplayHelpers.DisplayTitle();

    GameManager game = new();
    WorldMap map = game.Map;

    CanvasMapRenderer.Render(map);
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
}