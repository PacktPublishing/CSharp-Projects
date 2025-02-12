
using ConsoleRolePlayingGame.Domain.Combat;

namespace ConsoleRolePlayingGame.Domain.Repositories;

public class PartyRepository(AbilityRepository abilities) : FileRepositoryBase
{
    public Party Load()
    {
        IEnumerable<PlayerCharacter> characters =
            LoadManyFromJsonFile<PlayerCharacter>("Party.json")
                .Take(4)
                .Select(c => c with
                {
                    Health = c.MaxHealth,
                    Mana = c.MaxMana,
                    Abilities = abilities.GetAbilities(c.AbilityIds)
                });
        
        return new Party
        {
            Members = characters.Cast<GameCharacter>().ToList()
        };
    }
}