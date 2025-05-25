namespace ConsoleRolePlayingGame.ConsoleApp.Screens;

public class OverworldScreen(GameManager game, IAnsiConsole console)
{
    public const int Width = 21;
    private readonly Layout _layout = new Layout("Root").SplitRows(
        new Layout("Header").Size(1)
            .Update(new Markup("[bold yellow]Console Role Playing Game[/] by [cyan]Matt Eland[/]")),
        new Layout("Content").Size(Width)
            .SplitColumns(
                new Layout("Main").Size(Width * 2),
                new Layout("Sidebar")
            )
    );

    private readonly HelpRenderer _helpRenderer = new();
    private readonly MapRenderer _mapRenderer = new(game, Width, Width);
    private readonly PartyRenderer _partyRenderer = new(game.Party);
    
    public IRenderable GenerateVisual()
    {
        _layout["Main"].Update(_mapRenderer.GenerateVisual());
        _layout["Sidebar"].Update(new Rows(
            _partyRenderer.GenerateVisual(),
            _helpRenderer.GenerateVisual()
        ));

        return _layout;
    }

    public async Task HandlePlayerInputAsync()
    {
        console.Markup("[Yellow]>[/] ");
        ConsoleKeyInfo? keyInfo = await console.Input.ReadKeyAsync(true, CancellationToken.None);

        if (keyInfo.HasValue)
        {
            IMapEntity party = (IMapEntity) game.Party;
            switch (keyInfo.Value.Key)
            {
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    party.Move(Direction.West);
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    party.Move(Direction.East);
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    party.Move(Direction.South);
                    break;
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    party.Move(Direction.North);
                    break;
                case ConsoleKey.Q:
                case ConsoleKey.Escape:
                    game.Quit();
                    break;
            }
        }
    }
}