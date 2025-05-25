using ConsoleRolePlayingGame.Domain.Combat;
using ConsoleRolePlayingGame.Domain.Overworld;

namespace ConsoleRolePlayingGame.Domain.Repositories;

public class EncounterRepository(
    EnemyRepository enemyRepository, 
    Random random) : FileRepositoryBase
{
    public CombatGroup CreateRandomEncounter(Pos position)
    {
        List<EncounterInformation> encounters = LoadManyFromJsonFile<EncounterInformation>("Encounters.json");
        
        // Select a random element of encounters
        EncounterInformation encounter = encounters[random.Next(encounters.Count)];
        
        return new CombatGroup()
        {
            Name = encounter.Name,
            MapPos = position,
            Members = encounter.Enemies.SelectMany(e =>
            {
                Combatant enemy = enemyRepository.GetByName(e.Name);

                return Enumerable.Range(1, e.Count)
                    .Select(index => enemy with
                    {
                        Name = e.Count == 1 ? enemy.Name : $"{enemy.Name} {index}"
                    });
            })
            .ToList()
        };
    }
}