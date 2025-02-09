using ConsoleRolePlayingGame.ConsoleApp;
using ConsoleRolePlayingGame.Domain;
using ConsoleRolePlayingGame.Domain.Commands;
using ConsoleRolePlayingGame.Domain.Overworld;
using Spectre.Console;
using Spectre.Console.Rendering;

try
{
    AnsiConsole.Clear();
    
    GameManager game = new();
    
    List<GameCharacter> team = game.Party.Members;
    int maxHealth = team.Max(c => c.MaxHealth);
    int maxMana = team.Max(c => c.MaxMana);
    
    CanvasMapRenderer renderer = new(21, 21);

    Layout layout = new Layout("Root").SplitRows(
        new Layout("Header").Size(1),
        new Layout("Content").Size(renderer.Height)
            .SplitColumns(
                new Layout("Main").Size(renderer.Width * 2),
                new Layout("Sidebar")
            ),
        new Layout("Footer").Size(1));
    
    layout["Footer"].Update(new Markup("[Yellow]>[/] "));
    
    do
    {
        // Render a header with the party's current position
        Pos pos = game.Party.Position;
        layout["Header"].Update(
            new Markup($"Location: [cyan]{pos.X},{pos.Y}[/]. Press [yellow]q[/] to quit"));
        
        // Render the map
        layout["Main"].Update(renderer.Render(game));
        
        // Render the party status
        IEnumerable<IRenderable> partyMarkdown = team.Select(m => new Padder(
            new Rows(
                new Markup($"[bold]{m.Name}[/]"),
                new BarChart()
                    .AddItem("[Red]Health[/]", m.Health, Color.Red)
                    .AddItem("[Blue]Mana[/]", m.Mana, Color.Blue)
                    .WithMaxValue(Math.Max(maxHealth, maxMana))
                    .ShowValues()
            ), new Padding(0, 1, 0, 0))
        );
        layout["Sidebar"].Update(
                new Panel(new Rows(partyMarkdown))
                    .Header("[Yellow] Party Status [/]")
                    .Border(BoxBorder.Rounded)
                    .Expand()
        );
        
        AnsiConsole.Write(layout);

        ConsoleKeyInfo? keyInfo = AnsiConsole.Console.Input.ReadKey(intercept: true);

        AnsiConsole.WriteLine();
        AnsiConsole.Clear();

        if (keyInfo.HasValue)
        {
            Command? command = game.Commands.GetCommandForKey(keyInfo.Value.Key);
            command?.Execute(game);
        }
    } while (game.Status != GameStatus.Terminated);
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
}