using ConsoleRolePlayingGame.ConsoleApp.Visuals;
using ConsoleRolePlayingGame.Domain;
using ConsoleRolePlayingGame.Domain.Combat;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRolePlayingGame.ConsoleApp.Screens;

public class OverworldScreen(GameManager game, IAnsiConsole console) : IVisualGenerator
{
    private readonly InstructionsRenderer _helpRenderer = new();
    private readonly CanvasMapRenderer _mapRenderer = new(game, 21, 21);
    private readonly PartyMemberListRenderer _partyRenderer = new(game.Party);
    
    private readonly Layout _layout = new Layout("Root").SplitRows(
        new Layout("Header").Size(1).Update(new Markup("[bold yellow]Console Role Playing Game[/] by [cyan]Matt Eland[/]")),
        new Layout("Content").Size(21)
            .SplitColumns(
                new Layout("Main").Size(42),
                new Layout("Sidebar")
            )
    );

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
            CombatGroup party = game.Party;
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