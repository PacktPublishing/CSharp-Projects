using ConsoleRolePlayingGame.Domain.Combat;

namespace ConsoleRolePlayingGame.Domain.Repositories;

public class AbilityRepository : FileRepositoryBase
{
    public IEnumerable<Ability> GetAbilities() 
        => LoadManyFromJsonFile<Ability>("Abilities.json");

    public IEnumerable<Ability> GetAbilities(IEnumerable<string> ids)
    {
        IEnumerable<Ability> abilities = GetAbilities();
        return ids.Select(id => abilities.First(a => id == a.Id));
    }    
}