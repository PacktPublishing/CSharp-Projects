using ConsoleRolePlayingGame.ConsoleApp.Input;
using ConsoleRolePlayingGame.ConsoleApp.Screens;
using ConsoleRolePlayingGame.Domain;
using ConsoleRolePlayingGame.Domain.Combat;
using ConsoleRolePlayingGame.Domain.Overworld;
using ConsoleRolePlayingGame.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

IAnsiConsole console = AnsiConsole.Console;
try
{
    ServiceCollection services = new();
    
    // Singleton means one will be shared across the entire application
    services.AddSingleton<GameManager>();
    services.AddSingleton<IAnsiConsole>(console);
    services.AddSingleton<PerlinNoiseProvider>();
    services.AddSingleton<EncounterRepository>();
    services.AddSingleton<EnemyRepository>();
    services.AddSingleton<PartyRepository>();
    services.AddSingleton<AbilityRepository>();
    services.AddSingleton<MapGenerator>();
    services.AddSingleton<WorldMap>();
    services.AddSingleton<Random>();
    
    // Transients will be created each time they are requested
    services.AddTransient<ScreenManager>();
    services.AddTransient<OverworldScreen>();
    services.AddTransient<BattleScreen>();
    services.AddKeyedTransient<IBattleStrategy, EnemyTurnStrategy>(serviceKey: "Enemy");
    services.AddKeyedTransient<IBattleStrategy, PlayerTurnStrategy>(serviceKey: "Player");
    
    ServiceProvider provider = services.BuildServiceProvider();
    GameManager game = provider.GetRequiredService<GameManager>();
    ScreenManager screens = provider.GetRequiredService<ScreenManager>();
    
    while (game.Status != GameStatus.Terminated)
    {
        await screens.RunAsync();

        game.Update();
    }
}
catch (Exception ex)
{
    console.WriteException(ex, ExceptionFormats.ShortenEverything);
    console.Input.ReadKey(intercept: false);
}