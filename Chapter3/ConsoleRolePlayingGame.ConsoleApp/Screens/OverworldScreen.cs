using ConsoleRolePlayingGame.Overworld.Entities;
using ConsoleRolePlayingGame.Overworld.Structure;

namespace ConsoleRolePlayingGame.ConsoleApp.Screens;

public class OverworldScreen(GameManager game, IAnsiConsole console)
{
    public const int Size = 21;
    private readonly Layout _layout = new Layout("Root").SplitRows(
        new Layout("Header").Size(1)
            .Update(new Markup("[bold yellow]Console Role Playing Game[/] by [cyan]Matt Eland[/]")),
        new Layout("Content").Size(Size)
            .SplitColumns(
                new Layout("Main").Size(Size * 2),
                new Layout("Sidebar")
            )
    );

    private readonly HelpRenderer _helpRenderer = new();
    private readonly MapRenderer _mapRenderer = new(game, Size, Size);
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

    public void HandlePlayerInput()
    {
        console.Markup("[Yellow]>[/] ");
        ConsoleKeyInfo? keyInfo = console.Input.ReadKey(true);

        if (keyInfo.HasValue)
        {
            switch (keyInfo.Value.Key)
            {
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    game.MoveParty(Direction.West);
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    game.MoveParty(Direction.East);
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    game.MoveParty(Direction.South);
                    break;
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    game.MoveParty(Direction.North);
                    break;
                case ConsoleKey.Q:
                case ConsoleKey.Escape:
                    game.Quit();
                    break;
            }
        }
    }
}