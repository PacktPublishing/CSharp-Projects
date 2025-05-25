namespace ConsoleRolePlayingGame.Domain.Combat;

public interface IBattleStrategy
{
    string Execute(Battle battle, Combatant combatant);
}