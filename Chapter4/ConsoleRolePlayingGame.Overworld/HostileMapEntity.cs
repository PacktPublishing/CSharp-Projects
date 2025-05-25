namespace ConsoleRolePlayingGame.Overworld;

public class HostileMapEntity(Pos startPos) : IMapEntity
{
    public Pos MapPos { get; set; } = startPos;
    public EntityType EntityType => EntityType.Enemy;
}