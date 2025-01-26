using ConsoleAppAdventureGame.Content;
using ConsoleAppAdventureGame.Engine;
using ConsoleAppAdventureGame.Renderers;
using Spectre.Console;

try
{
    Adventure adventure = new SampleAdventure();
    SpectreConsoleAdventureRenderer renderer = new();
    adventure.Run(renderer);
    
    // Normal exit code
    return 0;
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
    
    // An error exit code
    return 1;
}