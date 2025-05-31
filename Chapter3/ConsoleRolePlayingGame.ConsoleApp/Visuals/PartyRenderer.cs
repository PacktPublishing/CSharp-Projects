namespace ConsoleRolePlayingGame.ConsoleApp.Visuals;

public class PartyRenderer(PlayerParty party)
{
    public IRenderable GenerateVisual()
    {
        IRenderable partyMarkdown =
            new Rows(
                new Markup("[bold]Hero[/]"),
                new Padder(
                    new BarChart()
                        .AddItem("[Red]HP[/]", party.Health, Color.Red)
                        .AddItem("[Blue]MP[/]", party.Mana, Color.Blue)
                        .WithMaxValue(PlayerParty.MaxStat)
                        .ShowValues()
                ));

        return new Panel(new Rows(partyMarkdown))
            .Header($"[Yellow] {party.Name} [/]")
            .Border(BoxBorder.Rounded);
    }
}