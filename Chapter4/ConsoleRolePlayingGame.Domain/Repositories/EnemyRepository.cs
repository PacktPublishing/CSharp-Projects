using ConsoleRolePlayingGame.Domain.Combat;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleRolePlayingGame.Domain.Repositories;

public class EnemyRepository([FromKeyedServices("Enemy")] IBattleStrategy strategy,
    AbilityRepository abilityRepository) : FileRepositoryBase
{
    public Combatant GetByName(string enemyName)
    {
        List<CombatantData> enemies = LoadManyFromJsonFile<CombatantData>("Enemies.json");

        CombatantData e = enemies.First(e => enemyName.Equals(e.Name, StringComparison.OrdinalIgnoreCase));

        return new Combatant(e)
        {
            IsPlayer = false,
            Strategy = strategy,
            Abilities = abilityRepository.GetAbilities(e.AbilityIds),
        };
    }
}