namespace ConsoleRolePlayingGame.Overworld;

public interface IMapEntity
{
    EntityType EntityType { get; }
    Pos MapPos { get; set; }

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