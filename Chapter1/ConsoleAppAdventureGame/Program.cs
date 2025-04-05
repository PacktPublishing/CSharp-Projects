using ConsoleAppAdventureGame.Renderers;
using ConsoleAppAdventureGame.Engine;
using Spectre.Console;
using ConsoleAppAdventureGame.Stories;

try
{
    Adventure adventure = SampleAdventure.BuildMinimalAdventureUsingBuilder();
    SpectreConsoleAdventureRenderer renderer = new();
    adventure.Run(renderer);
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
    throw;
}