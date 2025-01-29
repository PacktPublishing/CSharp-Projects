using ConsoleAppAdventureGame;
using ConsoleAppAdventureGame.Renderers;
using ConsoleAppAdventureGame.Engine;
using Spectre.Console;

try
{
    Adventure adventure = SampleAdventureBuilder.BuildSampleAdventure();
    SpectreConsoleAdventureRenderer renderer = new();
    adventure.Run(renderer);
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
    throw;
}