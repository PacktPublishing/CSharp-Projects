using ConsoleRolePlayingGame.Domain.Combat;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleRolePlayingGame.Domain.Repositories;

public class EnemyRepository([FromKeyedServices("Enemy")] IBattleStrategy strategy) : FileRepositoryBase
{
    public Combatant GetByName(string enemyName)
    {
        List<Combatant> enemies = LoadManyFromJsonFile<Combatant>("Enemies.json");

        Combatant enemy = enemies.First(e => enemyName.Equals(e.Name, StringComparison.OrdinalIgnoreCase));
        
        return enemy with
        {
            Health = enemy.MaxHealth,
            Mana = enemy.MaxMana,
            Strategy = strategy,
            IsPlayer = false
        };
    }
}