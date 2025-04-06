using ConsoleAppAdventureGame.Renderers;
using ConsoleAppAdventureGame.Engine;
using Spectre.Console;
using ConsoleAppAdventureGame.Stories;

try
{
    // Use rich encoding for Spectre.Console output to allow for bold and italic styling
    Console.OutputEncoding = System.Text.Encoding.UTF8;

    Adventure adventure = SampleAdventure.BuildAdventure();
    SpectreConsoleAdventureRenderer renderer = new();
    adventure.Run(renderer);
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
    throw;
}