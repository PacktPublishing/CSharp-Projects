using ConsoleRolePlayingGame.CombatSystem;
using ConsoleRolePlayingGame.Overworld;

namespace ConsoleRolePlayingGame.Domain.Entities;

public class EnemyGroup : IMapEntity, ICombatGroup
{
    public EntityType EntityType => EntityType.Enemy;
    public required Pos MapPos { get; set; }
    public required string Name { get; init; }
    public required List<Combatant> Members { get; init; }
}