namespace ConsoleRolePlayingGame.CombatSystem;

public interface IBattleStrategy
{
    string Execute(Battle battle, Combatant combatant);
}