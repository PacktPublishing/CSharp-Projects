using ConsoleRolePlayingGame.Overworld;
using ConsoleRolePlayingGame.Overworld.Entities;
using ConsoleRolePlayingGame.Overworld.Structure;

namespace ConsoleRolePlayingGame.Domain;

public class EnemyGroup(Pos pos) : IMapEntity
{
    public Pos MapPos { get; set; } = pos;
    public EntityType EntityType => EntityType.Enemy;

    public void MoveTowards(Pos target, WorldMap map)
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
        
        // Check if the next position is occupied
        Pos newPos = MapPos.Move(direction);
        if (map.Entities.All(e => e.MapPos != newPos || e != this))
        {
            MapPos = newPos;
        }
    }
}