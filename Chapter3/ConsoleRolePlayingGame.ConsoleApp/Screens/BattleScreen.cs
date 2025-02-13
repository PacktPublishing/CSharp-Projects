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
        
        _layout["Enemies"].Update(new CombatGroupRenderer(battle.Enemies, battle.ActiveMember).GenerateVisual());
        _layout["Party"].Update(new CombatGroupRenderer(battle.Party, battle.ActiveMember, includeStats: true).GenerateVisual());
        
        return _layout;
    }

    public async Task HandlePlayerInputAsync()
    {
        string? message = null;
        AnsiConsole.Cursor.SetPosition(0, 26);
        Battle battle = game.Battle!;

        if (battle.ActiveMember is null)
        {
            await AnsiConsole.Status()
                .StartAsync("Wait for next combatant...", async ctx =>
            {
                await Task.Delay(250);
                battle.AdvanceTime();
            });
        }
        else if (battle.ActiveMember is Enemy e)
        {
            message = battle.RunAiTurn(e);
        }
        else if (battle.ActiveMember is PlayerCharacter pc)
        {
            Ability ability = AnsiConsole.Prompt(new SelectionPrompt<Ability>()
                .Title($"Select an action for [yellow]{pc.Name}[/]")
                .AddChoices(pc.Abilities)
                .UseConverter(a => a.Name));
            
            IEnumerable<GameCharacter> targets = ability.IsHeal ? battle.Party.Members : battle.Enemies.Members;
            if (ability.TargetsSingle)
            {
                GameCharacter target = AnsiConsole.Prompt(new SelectionPrompt<GameCharacter>()
                    .Title("Select a target")
                    .PageSize(3)
                    .AddChoices(targets.Where(t => !t.IsDead))
                    .UseConverter(c => c.Name));

                targets = [target];
            }
            
            message = battle.RunTurn(pc, ability, targets);
        }
        
        // The user may need to see the results of their turn or the AI's actions
        if (!string.IsNullOrEmpty(message))
        {
            AnsiConsole.WriteLine(message);
            AnsiConsole.MarkupLine("[Yellow]Press any key to continue.[/] ");
            AnsiConsole.Console.Input.ReadKey(true);
        }
    }
}