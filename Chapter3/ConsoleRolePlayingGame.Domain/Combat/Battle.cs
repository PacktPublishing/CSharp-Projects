using System.Text;

namespace ConsoleRolePlayingGame.Domain.Combat;

public class Battle(CombatGroup party, CombatGroup enemies, Random random)
{
    public CombatGroup Enemies => enemies;
    public CombatGroup Party => party;
    
    public IEnumerable<Combatant> AllCharacters => [..party.Members, ..enemies.Members];
    
    public Combatant? ActiveMember => AllCharacters
            .Where(c => c.IsReady)
            .OrderBy(c => c.TimeUntilTurn)
            .FirstOrDefault();

    private const int TimeBetweenTurns = 100;
    
    public Battle Start()
    {
        foreach (var member in AllCharacters)
        {
            member.TimeUntilTurn = TimeBetweenTurns;
        }
        return this;
    }
    
    public void AdvanceTime()
    {
        foreach (var member in AllCharacters)
        {
            member.AdvanceTime();
        }
    }

    public string RunAiTurn(Combatant enemy)
    {
        // Select a random ability
        Ability ability = enemy.Abilities
            .OrderBy(_ => random.Next())
            .First();
        
        IEnumerable<Combatant> targets = ability.IsHeal
            ? Enemies.Members.Where(c => !c.IsDead).ToArray()
            : Party.Members.Where(c => !c.IsDead).ToArray();
      
        // Select a random target if the ability is targeted
        if (ability.TargetsSingle)
        {
            targets = targets
                .OrderBy(_ => random.Next())
                .Take(1);
        }
        
        // Execute the command
        return RunTurn(enemy, ability, targets);
    }

    public string RunTurn(Combatant character, Ability ability, IEnumerable<Combatant> targets)
    {
        if (ability.ManaCost > 0 && character.Mana < ability.ManaCost)
        {
            return $"{character.Name} does not have enough mana to use {ability.Name}!";
        }
        
        character.Mana -= ability.ManaCost;
        
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"{character.Name} uses {ability.Name}!");

        int multiplier = ability.Attribute switch
        {
            Trait.Strength => character.Strength,
            Trait.Dexterity => character.Dexterity,
            Trait.Intelligence => character.Intelligence,
            _ => 1
        };
            
        float amount = random.Next(
            (int)(ability.MinMultiplier * multiplier),
            (int)(ability.MaxMultiplier * multiplier));
        
        foreach (var target in targets)
        {
            if (target.IsDead)
            {
                continue;
            }
            
            if (ability.IsHeal)
            {
                target.Health = Math.Min(target.MaxHealth, target.Health + (int)amount);
                sb.AppendLine($"{target.Name} heals for {amount}!");
            }
            else
            {
                target.Health = Math.Max(0, target.Health - (int)amount);
                if (target.IsDead)
                {
                    sb.AppendLine($"{target.Name} takes {amount} damage and dies!");
                }
                else
                {
                    sb.AppendLine($"{target.Name} takes {amount} damage.");
                }
            }
        }

        character.TimeUntilTurn = TimeBetweenTurns;

        return sb.ToString();
    }
}