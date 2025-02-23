using ConsoleRolePlayingGame.Domain.Overworld;

namespace ConsoleRolePlayingGame.Domain.Combat;

public class CombatGroup
{
    public required string Name { get; init;  }
    public required Pos MapPos { get; set; }
    public required List<Combatant> Members { get; init; }
    public bool IsDead => Members.All(m => m.IsDead);
    
    public void Move(Direction direction)
    {
        MapPos = direction switch
        {
            Direction.North => MapPos with { Y = MapPos.Y - 1 },
            Direction.South => MapPos with { Y = MapPos.Y + 1 },
            Direction.East => MapPos with { X = MapPos.X + 1 },
            Direction.West => MapPos with { X = MapPos.X - 1 },
            _ => MapPos
        };
    }
}