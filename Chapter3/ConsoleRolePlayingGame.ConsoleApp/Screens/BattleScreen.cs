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
            battle.RunAiTurn(e);
        }
        else if (battle.ActiveMember is PlayerCharacter pc)
        {
            Ability ability = AnsiConsole.Prompt(new SelectionPrompt<Ability>()
                .Title($"Select an action for [yellow]{pc.Name}[/]")
                .AddChoices(pc.Abilities)
                .UseConverter(a => a.Name));

            GameCharacter? target = null;
            if (ability.TargetsSingle)
            {
                target = AnsiConsole.Prompt(new SelectionPrompt<GameCharacter>()
                    .Title("Select a target")
                    .AddChoices(ability.IsHeal ? battle.Party.Members : battle.Enemies.Members)
                    .UseConverter(c => c.Name));
            }
            
            battle.RunTurn(pc, ability, target);
        }
    }
}