namespace ConsoleRolePlayingGame.Domain.Combat;

public class EnemyTurnStrategy(Random random) : IBattleStrategy
{
    public string Execute(Battle battle, Combatant combatant)
    {
        // Select a random ability
        Ability ability = combatant.Abilities
            .OrderBy(_ => random.Next())
            .First();
        
        IEnumerable<Combatant> targets = ability.IsHeal
            ? battle.Enemies.Members.Where(c => !c.IsDead)
            : battle.Party.Members.Where(c => !c.IsDead);
      
        // Select a random target if the ability is targeted
        if (ability.TargetsSingle)
        {
            targets = targets
                .OrderBy(_ => random.Next())
                .Take(1);
        }
        
        return battle.RunTurn(combatant, ability, targets);
    }
}