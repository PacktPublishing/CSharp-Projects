using ConsoleRolePlayingGame.ConsoleApp.Visuals;
using ConsoleRolePlayingGame.Domain;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRolePlayingGame.ConsoleApp.Screens;

public class OverworldScreen : IVisualGenerator
{
    private readonly InstructionsRenderer _helpRenderer;
    private readonly CanvasMapRenderer _mapRenderer;
    private readonly PartyMemberListRenderer _partyRenderer;
    private readonly Layout _layout;

    public OverworldScreen(GameManager game)
    {
        // Instantiate our renderers that will be used to render the UI
        _partyRenderer = new PartyMemberListRenderer(game.Party.Members);
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
}