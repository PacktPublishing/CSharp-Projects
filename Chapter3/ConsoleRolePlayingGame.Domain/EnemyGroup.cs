using ConsoleRolePlayingGame.Domain.Overworld;

namespace ConsoleRolePlayingGame.Domain;

public class EnemyGroup(Pos pos) : IMapEntity
{
    public Pos Position { get; set; } = pos;
}