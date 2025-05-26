using ConsoleRolePlayingGame.Domain.Entities;
using ConsoleRolePlayingGame.Overworld.Generators;

IAnsiConsole console = AnsiConsole.Console;
try
{
    ServiceCollection services = new();
    
    // Singleton means one will be shared across the entire application
    services.AddSingleton<GameManager>();
    services.AddSingleton<IAnsiConsole>(console);
    services.AddSingleton<PerlinNoiseProvider>();
    services.AddSingleton<MapGenerator>();
    services.AddSingleton<WorldMap>();
    services.AddSingleton<OpenPosSelector>();
    services.AddSingleton<PlayerParty>();
    
    // Transients will be created each time they are requested
    services.AddTransient<ScreenManager>();
    services.AddTransient<OverworldScreen>();
    
    ServiceProvider provider = services.BuildServiceProvider();
    GameManager game = provider.GetRequiredService<GameManager>();
    ScreenManager screens = provider.GetRequiredService<ScreenManager>();
    
    while (game.Status != GameStatus.Terminated)
    {
        screens.Run();
        game.Update();
    }
}
catch (Exception ex)
{
    console.WriteException(ex, ExceptionFormats.ShortenEverything);
    console.Input.ReadKey(intercept: false);
}