using ConsoleRolePlayingGame.ConsoleApp.Visuals;
using ConsoleRolePlayingGame.Domain;
using ConsoleRolePlayingGame.Domain.Commands;
using Spectre.Console;

try
{
    GameManager game = new();
    
    // Instantiate our renderers that will be used to render the UI
    PartyMemberListRenderer partyStatsRenderer = new(game.Party.Members);
    CanvasMapRenderer mapRenderer = new(game, 21, 21);
    InstructionsRenderer helpRenderer = new();

    // Build out the structure of the main layout as well as the parts that won't update
    Layout layout = new Layout("Root").SplitRows(
        new Layout("Header").Size(1),
        new Layout("Content").Size(mapRenderer.Height)
            .SplitColumns(
                new Layout("Main").Size(mapRenderer.Width * 2),
                new Layout("Sidebar")
            )
    );
    layout["Header"].Update(new Markup("[bold yellow]Console Role Playing Game[/] by [cyan]Matt Eland[/]"));

    while (game.Status != GameStatus.Terminated)
    {
        // Ensures the app is rendered at a consistent size
        AnsiConsole.Clear();
        
        // Render the map
        layout["Main"].Update(mapRenderer.GenerateVisual());

        // Render the party status
        layout["Sidebar"].Update(new Rows(
                partyStatsRenderer.GenerateVisual(),
                helpRenderer.GenerateVisual()
        ));

        // Get input from the user
        AnsiConsole.Write(layout);
        AnsiConsole.Markup("[Yellow]>[/] ");
        ConsoleKeyInfo? keyInfo = AnsiConsole.Console.Input.ReadKey(intercept: true);

        if (keyInfo.HasValue)
        {
            Command? command = game.Commands.GetCommandForKey(keyInfo.Value.Key);
            command?.Execute(game);
        }
    }
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
}