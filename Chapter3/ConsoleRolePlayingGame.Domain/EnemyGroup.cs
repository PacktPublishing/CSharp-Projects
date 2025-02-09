using ConsoleRolePlayingGame.Domain.Overworld;

namespace ConsoleRolePlayingGame.Domain;

public abstract class EnemyGroup(Pos pos) : IMapEntity
{
    public Pos Position { get; set; } = pos;
}