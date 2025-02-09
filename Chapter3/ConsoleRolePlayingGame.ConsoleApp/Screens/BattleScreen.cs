using ConsoleRolePlayingGame.ConsoleApp.Visuals;
using ConsoleRolePlayingGame.Domain;
using ConsoleRolePlayingGame.Domain.Combat;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRolePlayingGame.ConsoleApp.Screens;

public class BattleScreen(GameManager game) : IVisualGenerator
{
    private readonly Layout _layout = new Layout()
        .SplitRows(
            new Layout().SplitColumns(
                new Layout("Enemies").Ratio(3),
                new Layout("Party").Ratio(2)
            ).Size(25)
        );

    public IRenderable GenerateVisual()
    {
        Battle battle = game.Battle!;
        
        _layout["Enemies"].Update(new CombatGroupRenderer(battle.Enemies).GenerateVisual());
        _layout["Party"].Update(new CombatGroupRenderer(battle.Party, includeStats: true).GenerateVisual());
        
        return _layout;
    }
}