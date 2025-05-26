using ConsoleRolePlayingGame.CombatSystem;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleRolePlayingGame.Domain.Repositories;

public class EnemyRepository([FromKeyedServices("Enemy")] IBattleStrategy strategy,
    AbilityRepository abilityRepository) : FileRepositoryBase
{
    public Combatant GetByName(string enemyName)
    {
        List<CombatantData> enemies = LoadManyFromJsonFile<CombatantData>("Enemies.json");

        CombatantData e = enemies.First(e => enemyName.Equals(e.Name, StringComparison.OrdinalIgnoreCase));

        return new Combatant(e, strategy)
        {
            IsPlayer = false,
            Abilities = abilityRepository.GetAbilities(e.AbilityIds),
        };
    }
}