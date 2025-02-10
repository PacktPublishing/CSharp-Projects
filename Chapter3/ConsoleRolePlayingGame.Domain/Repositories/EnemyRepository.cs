using ConsoleRolePlayingGame.Domain.Combat;

namespace ConsoleRolePlayingGame.Domain.Repositories;

public class EnemyRepository : FileRepositoryBase
{
    public Enemy GetByName(string enemyName)
    {
        List<Enemy> enemies = LoadManyFromJsonFile<Enemy>("Enemies.json");

        Enemy enemy = enemies.First(e => enemyName.Equals(e.Name, StringComparison.OrdinalIgnoreCase));
        
        return enemy with
        {
            Health = enemy.MaxHealth,
            Mana = enemy.MaxMana
        };
    }
}