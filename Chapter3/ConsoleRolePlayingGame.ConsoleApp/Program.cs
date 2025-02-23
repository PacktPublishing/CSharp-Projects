using ConsoleRolePlayingGame.ConsoleApp.Input;
using ConsoleRolePlayingGame.ConsoleApp.Screens;
using ConsoleRolePlayingGame.Domain;
using ConsoleRolePlayingGame.Domain.Combat;
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
    services.AddSingleton<IAnsiConsole>(_ => AnsiConsole.Console);
    services.AddScoped<PerlinNoiseProvider>();
    services.AddScoped<EncounterRepository>();
    services.AddScoped<EnemyRepository>();
    services.AddScoped<PartyRepository>();
    services.AddScoped<AbilityRepository>();
    services.AddScoped<MapGenerator>();
    services.AddScoped<WorldMap>();
    services.AddScoped<Random>();
    services.AddKeyedScoped<IBattleStrategy, EnemyTurnStrategy>("Enemy");
    services.AddKeyedScoped<IBattleStrategy, PlayerTurnStrategy>("Player");
    
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
                await overworldScreen.HandlePlayerInputAsync();
                break;
            
            case GameStatus.Combat:
                BattleScreen battleScreen = provider.GetRequiredService<BattleScreen>();
                AnsiConsole.Write(battleScreen.GenerateVisual());
                await battleScreen.HandlePlayerInputAsync();
                break;         
            
            case GameStatus.GameOver:
                AnsiConsole.MarkupLine("[red]Game Over[/]");
                AnsiConsole.MarkupLine("[yellow]Press any key to exit...[/]");
                Console.ReadKey();
                game.Quit();
                break;
        }

        game.Update();
    }
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
    Console.ReadKey();
}