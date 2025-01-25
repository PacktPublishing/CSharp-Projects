using ConsoleAppAdventureGame.Content;
using ConsoleAppAdventureGame.Engine;

Adventure adventure = new SampleAdventure();

SpectreConsoleAdventureRenderer renderer = new();
adventure.Run(renderer);

