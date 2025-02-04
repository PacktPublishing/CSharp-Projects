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
    Canvas canvas = new(map.Width, map.Height);
    foreach (var cell in map.Map)
    {
        canvas.SetPixel(cell.Position.X, cell.Position.Y, cell.Terrain.ToColor());
    }
    
    canvas.SetPixel(map.PlayerPos.X, map.PlayerPos.Y, Color.Yellow1);
    
    AnsiConsole.Write(canvas);
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
}