using ConsoleRolePlayingGame.Domain.Overworld;

namespace ConsoleRolePlayingGame.Domain;

public class PlayerCharacter(Pos pos) : GameCharacter(pos)
{
    public void Move(Direction direction)
    {
        Position = direction switch
        {
            Direction.North => Position with { Y = Position.Y - 1 },
            Direction.South => Position with { Y = Position.Y + 1 },
            Direction.East => Position with { X = Position.X + 1 },
            Direction.West => Position with { X = Position.X - 1 },
            _ => Position
        };
    }
}