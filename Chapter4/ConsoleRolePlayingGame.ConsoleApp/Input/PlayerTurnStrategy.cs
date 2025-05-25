using ConsoleRolePlayingGame.Domain.Combat;
using Spectre.Console;

namespace ConsoleRolePlayingGame.ConsoleApp.Input;

public class PlayerTurnStrategy(IAnsiConsole console) : IBattleStrategy
{
    public string Execute(Battle battle, Combatant combatant)
    {
        Ability ability = console.Prompt(new SelectionPrompt<Ability>()
            .Title($"Select an action for [yellow]{combatant.Name}[/]")
            .AddChoices(combatant.Abilities)
            .UseConverter(a => a.Name));
            
        IEnumerable<Combatant> targets = ability.IsHeal ? battle.Party.Members : battle.Enemies.Members;
        if (ability.TargetsSingle)
        {
            Combatant target = console.Prompt(new SelectionPrompt<Combatant>()
                .Title("Select a target")
                .PageSize(3)
                .AddChoices(targets.Where(t => !t.IsDead))
                .UseConverter(c => c.Name));

            targets = [target];
        }
            
        return battle.RunTurn(combatant, ability, targets);
    }
}