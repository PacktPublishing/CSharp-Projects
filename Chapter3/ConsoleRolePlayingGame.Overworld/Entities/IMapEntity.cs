using ConsoleRolePlayingGame.Overworld.Structure;

namespace ConsoleRolePlayingGame.Overworld.Entities;

public interface IMapEntity
{
    EntityType EntityType { get; }
    Pos MapPos { get; set; }

    public void Move(Direction direction)
    {
        MapPos = MapPos.Move(direction);
    }
}