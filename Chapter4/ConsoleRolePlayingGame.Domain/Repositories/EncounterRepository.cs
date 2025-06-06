using ConsoleRolePlayingGame.CombatSystem;
using ConsoleRolePlayingGame.Domain.Entities;
using ConsoleRolePlayingGame.Overworld.Structure;

namespace ConsoleRolePlayingGame.Domain.Repositories;

public class EncounterRepository(EnemyRepository enemyRepository) : FileRepositoryBase, IEncounterProvider
{
    public Random Random { get; set; } = Random.Shared;
    
    public EnemyGroup CreateRandomEncounter(Pos position)
    {
        List<EncounterInformation> encounters = LoadManyFromJsonFile<EncounterInformation>("Encounters.json");
        
        // Select a random element of encounters
        EncounterInformation encounter = encounters[Random.Next(encounters.Count)];
        
        return new EnemyGroup()
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