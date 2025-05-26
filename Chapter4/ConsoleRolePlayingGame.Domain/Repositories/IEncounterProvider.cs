using ConsoleRolePlayingGame.Domain.Entities;
using ConsoleRolePlayingGame.Overworld.Structure;

namespace ConsoleRolePlayingGame.Domain.Repositories;

public interface IEncounterProvider
{
    EnemyGroup CreateRandomEncounter(Pos position);
}