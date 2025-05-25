namespace ConsoleRolePlayingGame.Overworld.Structure;

public record Pos(int X, int Y)
{
    public Pos Move(Direction direction)
    {
        return direction switch
        {
            Direction.North => this with { Y = Y - 1 },
            Direction.South => this with { Y = Y + 1 },
            Direction.East => this with { X = X + 1 },
            Direction.West => this with { X = X - 1 },
            _ => this
        };
    }
}