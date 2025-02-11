using ConsoleRolePlayingGame.Domain.Combat;

namespace ConsoleRolePlayingGame.Domain.Repositories;

public class AbilityRepository : FileRepositoryBase
{
    public IEnumerable<Ability> GetAbilities(IEnumerable<string> ids)
    {
        List<Ability> abilities = LoadManyFromJsonFile<Ability>("Abilities.json");

        return ids.Select(id => abilities.First(a => id == a.Id));
    }
}