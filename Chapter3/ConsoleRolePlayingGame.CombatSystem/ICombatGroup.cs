namespace ConsoleRolePlayingGame.CombatSystem;

public interface ICombatGroup
{
    string Name { get; init;  }
    List<Combatant> Members { get; init; }
}