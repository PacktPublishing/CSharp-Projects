namespace ConsoleRolePlayingGame.ConsoleApp.Screens;

public class BattleScreen(GameManager game, IAnsiConsole console)
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
        Battle? battle = game.Battle;
        if (battle is null) throw new InvalidOperationException("No active battle to render.");
        
        _layout["Enemies"].Update(new CombatGroupRenderer(battle.Enemies, battle.ActiveMember).GenerateVisual());
        _layout["Party"].Update(new CombatGroupRenderer(battle.Party, battle.ActiveMember, includeStats: true).GenerateVisual());
        
        return _layout;
    }

    public async Task HandlePlayerInputAsync()
    {
        string? message = null;
        console.Cursor.SetPosition(0, 26);
        Battle battle = game.Battle!;

        Combatant? combatant = battle.ActiveMember;
        if (combatant is null)
        {
            await console.Status()
                .StartAsync("Wait for next combatant...", async _ =>
            {
                await Task.Delay(250);
                battle.AdvanceTime();
            });
        }
        else
        {
            message = combatant.Strategy.Execute(battle, combatant);
        }
        
        // The user may need to see the results of their turn or the AI's actions
        if (!string.IsNullOrEmpty(message))
        {
            console.WriteLine(message);
            console.MarkupLine("[Yellow]Press any key to continue.[/] ");
            console.Input.ReadKey(true);
        }
    }
}