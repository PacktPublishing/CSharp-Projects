using System.Text;

namespace ConsoleRolePlayingGame.CombatSystem;

public class Battle
{
    private readonly ICombatGroup _party;
    private readonly ICombatGroup _enemies;
    private readonly Random _random;

    public Battle(ICombatGroup party, ICombatGroup enemies, Random random)
    {
        _party = party;
        _enemies = enemies;
        _random = random;
        
        foreach (var member in AllCharacters)
        {
            member.TimeUntilTurn = TimeBetweenTurns;
        }
    }

    public ICombatGroup Enemies => _enemies;
    public ICombatGroup Party => _party;
    public IEnumerable<Combatant> AllCharacters => [.._party.Members, .._enemies.Members];

    public Combatant? ActiveMember => AllCharacters
        .Where(c => c.IsReady)
        .OrderBy(c => c.TimeUntilTurn)
        .FirstOrDefault();

    private const int TimeBetweenTurns = 100;

    public void AdvanceTime()
    {
        foreach (var member in AllCharacters.Where(m => !m.IsDead))
        {
            member.TimeUntilTurn -= member.Speed;
        }
    }

    public string RunTurn(Combatant character, Ability ability, IEnumerable<Combatant> targets)
    {
        if (ability.ManaCost > 0 && character.Mana < ability.ManaCost)
        {
            return $"{character.Name} does not have enough mana to use {ability.Name}!";
        }
        
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"{character.Name} uses {ability.Name}!");
        
        character.Mana -= ability.ManaCost;
        character.TimeUntilTurn = TimeBetweenTurns;

        foreach (var target in targets.Where(t => !t.IsDead))
        {
            string message = ActivateAbilityOnTarget(character, ability, target);
            sb.AppendLine(message);
        }

        return sb.ToString();
    }

    private string ActivateAbilityOnTarget(Combatant character, Ability ability, Combatant target)
    {
        int amount = ability.CalculateAmount(character, target, _random);

        if (ability.IsHeal)
        {
            target.Health = Math.Min(target.MaxHealth, target.Health + (int)amount);
            return $"{target.Name} heals for {amount}!";
        }

        target.Health = Math.Max(0, target.Health - (int)amount);
        return target.IsDead
            ? $"{target.Name} takes {amount} damage and dies!"
            : $"{target.Name} takes {amount} damage.";
    }
}