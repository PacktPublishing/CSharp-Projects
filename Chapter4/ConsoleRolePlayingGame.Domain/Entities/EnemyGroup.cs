using ConsoleRolePlayingGame.CombatSystem;
using ConsoleRolePlayingGame.Overworld;
using ConsoleRolePlayingGame.Overworld.Entities;
using ConsoleRolePlayingGame.Overworld.Structure;

namespace ConsoleRolePlayingGame.Domain.Entities;

public class EnemyGroup : IMapEntity, ICombatGroup
{
    public EntityType EntityType => EntityType.Enemy;
    public required Pos MapPos { get; set; }
    public required string Name { get; init; }
    public required List<Combatant> Members { get; init; }

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