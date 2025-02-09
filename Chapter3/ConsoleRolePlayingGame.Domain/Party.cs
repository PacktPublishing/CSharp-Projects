using ConsoleRolePlayingGame.Domain.Combat;
using ConsoleRolePlayingGame.Domain.Overworld;

namespace ConsoleRolePlayingGame.Domain;

public class Party : IMapEntity, ICombatGroup
{
    public Pos Position { get; set; } = new(0, 0);
    public required List<GameCharacter> Members { get; init; }

    public string Name => "The Party";

    public bool IsDead => Members.All(m => m.IsDead);
}