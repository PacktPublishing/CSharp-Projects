using ConsoleRolePlayingGame.Domain;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRolePlayingGame.ConsoleApp.Visuals;

public class PartyMemberListRenderer(IEnumerable<PlayerCharacter> team) : IVisualGenerator
{
    public IRenderable GenerateVisual()
    {
        int maxHealth = team.Max(c => c.MaxHealth);
        int maxMana = team.Max(c => c.MaxMana);
        
        IEnumerable<IRenderable> partyMarkdown = team.Select(m => 
            new Rows(
                new Markup($"[bold]{m.Name}[/]"),
                new Padder(
                new BarChart()
                    .AddItem("[Red]Health[/]", m.Health, Color.Red)
                    .AddItem("[Blue]Mana[/]", m.Mana, Color.Blue)
                    .WithMaxValue(Math.Max(maxHealth, maxMana))
                    .ShowValues()
            , new Padding(5, 0, 0, 0)))
        );

        return new Panel(new Rows(partyMarkdown))
            .Header("[Yellow] Party Status [/]")
            .Border(BoxBorder.Rounded);
    }
}