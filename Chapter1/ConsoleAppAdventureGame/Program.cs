using ConsoleAppAdventureGame;
using ConsoleAppAdventureGame.Renderers;
using ConsoleAppAdventureGame.Engine;
using Spectre.Console;

const int normalExitCode = 0;
const int errorExitCode = 1;

try
{
    Adventure adventure = SampleAdventureBuilder.BuildSampleAdventure();
    SpectreConsoleAdventureRenderer renderer = new();
    adventure.Run(renderer);
    
    return normalExitCode;
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
    return errorExitCode;
}