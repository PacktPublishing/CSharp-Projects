using ConsoleRolePlayingGame.ConsoleApp.Screens;
using ConsoleRolePlayingGame.Domain;
using ConsoleRolePlayingGame.Domain.Commands;
using ConsoleRolePlayingGame.Domain.Overworld;
using ConsoleRolePlayingGame.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

try
{
    ServiceCollection services = new();
    services.AddSingleton<GameManager>();
    services.AddSingleton<OverworldScreen>();
    services.AddSingleton<BattleScreen>();
    services.AddScoped<PerlinNoiseProvider>();
    services.AddScoped<EncounterRepository>();
    services.AddScoped<EnemyRepository>();
    services.AddScoped<PartyRepository>();
    services.AddScoped<AbilityRepository>();
    services.AddScoped<MapGenerator>();
    services.AddScoped<WorldMap>();
    services.AddScoped<Random>();
    
    ServiceProvider provider = services.BuildServiceProvider();

    GameManager game = provider.GetRequiredService<GameManager>();

    while (game.Status != GameStatus.Terminated)
    {
        // Ensures the app is rendered at a consistent size
        AnsiConsole.Clear();

        switch (game.Status)
        {
            case GameStatus.Overworld:
                OverworldScreen overworldScreen = provider.GetRequiredService<OverworldScreen>();
                AnsiConsole.Write(overworldScreen.GenerateVisual());
                break;
            
            case GameStatus.Combat:
                BattleScreen battleScreen = provider.GetRequiredService<BattleScreen>();
                AnsiConsole.Write(battleScreen.GenerateVisual());
                break;         
            
            case GameStatus.GameOver:
                AnsiConsole.MarkupLine("[red]Game Over[/]");
                break;
        }

        // Get input from the user
        // TODO: This is probably only needed on the overworld
        AnsiConsole.Markup("[Yellow]>[/] ");
        ConsoleKeyInfo? keyInfo = AnsiConsole.Console.Input.ReadKey(intercept: true);

        if (keyInfo.HasValue)
        {
            // TODO: Commands should be specific to the current game state
            Command? command = game.Commands.GetCommandForKey(keyInfo.Value.Key);
            command?.Execute(game);
        }

        game.Update();
    }
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
    Console.ReadKey();
}