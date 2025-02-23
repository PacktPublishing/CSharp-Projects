using ConsoleRolePlayingGame.Domain.Combat;

namespace ConsoleRolePlayingGame.Domain.Repositories;

public class EnemyRepository : FileRepositoryBase
{
    public Combatant GetByName(string enemyName)
    {
        List<Combatant> enemies = LoadManyFromJsonFile<Combatant>("Enemies.json");

        Combatant enemy = enemies.First(e => enemyName.Equals(e.Name, StringComparison.OrdinalIgnoreCase));
        
        return enemy with
        {
            Health = enemy.MaxHealth,
            Mana = enemy.MaxMana,
            IsPlayer = false
        };
    }
}