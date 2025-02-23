
using ConsoleRolePlayingGame.Domain.Combat;
using ConsoleRolePlayingGame.Domain.Overworld;

namespace ConsoleRolePlayingGame.Domain.Repositories;

public class PartyRepository(AbilityRepository abilities) : FileRepositoryBase
{
    public CombatGroup Load() => new CombatGroup()
        {
            Name = "The Party",
            MapPos = new Pos(0, 0),
            Members = LoadManyFromJsonFile<Combatant>("Party.json")
                .Select(c => c with
                {
                    Health = c.MaxHealth,
                    Mana = c.MaxMana,
                    IsPlayer = true,
                    Abilities = abilities.GetAbilities(c.AbilityIds)
                }).ToList(),

        };
}