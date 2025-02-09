namespace ConsoleRolePlayingGame.Domain.Combat;

public class Battle(Party party, EnemyGroup enemies)
{
    public EnemyGroup Enemies => enemies;
    public Party Party => party;
}