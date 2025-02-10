using ConsoleRolePlayingGame.Domain.Combat;
using ConsoleRolePlayingGame.Domain.Overworld;

namespace ConsoleRolePlayingGame.Domain.Repositories;

public class EncounterRepository(EnemyRepository enemyRepository) : FileRepositoryBase
{
    public EnemyGroup CreateRandomEncounter(Pos position)
    {
        List<EncounterInformation> encounters = LoadManyFromJsonFile<EncounterInformation>("Data/Encounters.json");
        
        // Select a random element of encounters
        EncounterInformation encounter = encounters[Random.Shared.Next(encounters.Count)];
        
        return new EnemyGroup(position)
        {
            Name = encounter.Name,
            Members = encounter.Enemies.SelectMany(e =>
            {
                Enemy enemyTemplate = enemyRepository.GetByName(e.Name);

                return Enumerable.Range(1, e.Count)
                    .Select(index => enemyTemplate with
                    {
                        Name = e.Count == 1 
                            ? enemyTemplate.Name 
                            : $"{enemyTemplate.Name} {index}"
                    });
            })
            .Cast<GameCharacter>()
            .ToList()
        };
    }
}