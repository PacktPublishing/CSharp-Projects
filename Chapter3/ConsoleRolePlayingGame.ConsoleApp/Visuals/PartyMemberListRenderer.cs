using ConsoleRolePlayingGame.Domain;
using ConsoleRolePlayingGame.Domain.Combat;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRolePlayingGame.ConsoleApp.Visuals;

public class PartyMemberListRenderer(Party party) : IVisualGenerator
{
    public IRenderable GenerateVisual()
    {
        IEnumerable<GameCharacter> team = party.Members;
        int maxHealth = team.Max(c => c.MaxHealth);
        int maxMana = team.Max(c => c.MaxMana);
        
        IEnumerable<IRenderable> partyMarkdown = team.Select(m => 
            new Rows(
                new Markup($"[bold]{m.Name}[/]"),
                new Padder(
                new BarChart()
                    .AddItem("[Red]HP[/]", m.Health, Color.Red)
                    .AddItem("[Blue]MP[/]", m.Mana, Color.Blue)
                    .WithMaxValue(Math.Max(maxHealth, maxMana))
                    .ShowValues()
            , new Padding(5, 0, 0, 0)))
        );

        return new Panel(new Rows(partyMarkdown))
            .Header($"[Yellow] {party.Name} [/]")
            .Border(BoxBorder.Rounded);
    }
}