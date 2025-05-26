using ConsoleRolePlayingGame.Domain.Entities;

namespace ConsoleRolePlayingGame.ConsoleApp.Visuals;

public class PartyRenderer(PlayerParty party)
{
    public const int MaxStat = 10;
    public int Health { get; set; } = MaxStat;
    public int Mana { get; set; } = MaxStat;
    public IRenderable GenerateVisual()
    {
        IRenderable partyMarkdown =
            new Rows(
                new Markup($"[bold]Hero[/]"),
                new Padder(
                    new BarChart()
                        .AddItem("[Red]HP[/]", Health, Color.Red)
                        .AddItem("[Blue]MP[/]", Mana, Color.Blue)
                        .WithMaxValue(MaxStat)
                        .ShowValues()
                ));

        return new Panel(new Rows(partyMarkdown))
            .Header($"[Yellow] {party.Name} [/]")
            .Border(BoxBorder.Rounded);
    }
}