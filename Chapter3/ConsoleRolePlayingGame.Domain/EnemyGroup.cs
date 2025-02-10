using ConsoleRolePlayingGame.Domain.Combat;
using ConsoleRolePlayingGame.Domain.Overworld;

namespace ConsoleRolePlayingGame.Domain;

public class EnemyGroup(Pos pos) : IMapEntity, ICombatGroup
{
    public Pos Position { get; set; } = pos;
    public bool IsDead => Members.All(x => x.IsDead);
    public required List<GameCharacter> Members { get; init; }

    public string Name { get; init; } = "Enemies";
}