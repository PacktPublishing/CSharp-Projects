using ConsoleRolePlayingGame.Overworld.Entities;
using ConsoleRolePlayingGame.Overworld.Structure;

namespace ConsoleRolePlayingGame.Domain.Entities;

public class EnemyGroup(Pos pos) : IMapEntity
{
    public Pos MapPos { get; set; } = pos;
    public EntityType EntityType => EntityType.Enemy;

    public void MoveTowards(Pos target)
    {
        if (target == MapPos)
        {
            return;
        }
        
        int xDiff = target.X - MapPos.X;
        int yDiff = target.Y - MapPos.Y;

        Direction direction;
        if (Math.Abs(xDiff) > Math.Abs(yDiff))
        {
            direction = xDiff < 0 ? Direction.West : Direction.East;
        }
        else
        {
            direction = yDiff < 0 ? Direction.North : Direction.South;
        }
        
        MapPos = MapPos.Move(direction);
    }
}