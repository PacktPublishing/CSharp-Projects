namespace ConsoleRolePlayingGame.Domain.Combat;

public interface ICombatGroup
{
    bool IsDead { get; }
    List<GameCharacter> Members { get; }
    string Name { get; }
}