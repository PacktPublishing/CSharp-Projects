using ConsoleRolePlayingGame.ConsoleApp.Visuals;
using ConsoleRolePlayingGame.Domain;
using ConsoleRolePlayingGame.Domain.Combat;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRolePlayingGame.ConsoleApp.Screens;

public class OverworldScreen : IVisualGenerator
{
    private readonly GameManager _game;
    private readonly InstructionsRenderer _helpRenderer;
    private readonly CanvasMapRenderer _mapRenderer;
    private readonly PartyMemberListRenderer _partyRenderer;
    private readonly Layout _layout;

    public OverworldScreen(GameManager game)
    {
        _game = game;
        
        // Instantiate our renderers that will be used to render the UI
        _partyRenderer = new PartyMemberListRenderer(game.Party);
        _mapRenderer = new CanvasMapRenderer(game, 21, 21);
        _helpRenderer = new InstructionsRenderer();

        // Build out the structure of the main layout as well as the parts that won't update
        _layout = new Layout("Root").SplitRows(
            new Layout("Header").Size(1),
            new Layout("Content").Size(_mapRenderer.Height)
                .SplitColumns(
                    new Layout("Main").Size(_mapRenderer.Width * 2),
                    new Layout("Sidebar")
                )
        );
        _layout["Header"].Update(new Markup("[bold yellow]Console Role Playing Game[/] by [cyan]Matt Eland[/]"));
    }

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
        AnsiConsole.Markup("[Yellow]>[/] ");
        ConsoleKeyInfo? keyInfo = await AnsiConsole.Console.Input.ReadKeyAsync(true, CancellationToken.None);

        if (keyInfo.HasValue)
        {
            CombatGroup party = _game.Party;
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
                    _game.Quit();
                    break;
            }
        }
    }
}