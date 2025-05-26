namespace ConsoleRolePlayingGame.CombatSystem;

public class EnemyTurnStrategy : IBattleStrategy
{
    public Random Random { get; set; } = Random.Shared;
    
    public string Execute(Battle battle, Combatant combatant)
    {
        // Select a random ability
        Ability ability = combatant.Abilities
            .OrderBy(_ => Random.Next())
            .First();
        
        IEnumerable<Combatant> targets = ability.IsHeal
            ? battle.Enemies.Members.Where(c => !c.IsDead)
            : battle.Party.Members.Where(c => !c.IsDead);
      
        // Select a random target if the ability is targeted
        if (ability.TargetsSingle)
        {
            targets = targets
                .OrderBy(_ => Random.Next())
                .Take(1);
        }
        
        return battle.RunTurn(combatant, ability, targets);
    }
}