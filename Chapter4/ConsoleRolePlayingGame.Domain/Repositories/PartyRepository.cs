using ConsoleRolePlayingGame.CombatSystem;
using ConsoleRolePlayingGame.Domain.Entities;
using ConsoleRolePlayingGame.Overworld;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleRolePlayingGame.Domain.Repositories;

public class PartyRepository(AbilityRepository abilities, [FromKeyedServices("Player")] IBattleStrategy strategy) : FileRepositoryBase
{
    public PlayerParty Load() => new()
        {
            Name = "The Party",
            MapPos = new Pos(0, 0),
            Members = LoadManyFromJsonFile<CombatantData>("Party.json")
                .Select(c => new Combatant(c)
                {
                    IsPlayer = true,
                    Strategy = strategy,
                    Abilities = abilities.GetAbilities(c.AbilityIds),
                }).ToList(),

        };
}