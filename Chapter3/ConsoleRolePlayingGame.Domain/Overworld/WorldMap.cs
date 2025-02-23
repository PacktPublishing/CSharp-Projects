using ConsoleRolePlayingGame.Domain.Combat;

namespace ConsoleRolePlayingGame.Domain.Overworld;

public class WorldMap(MapGenerator mapGenerator, Random random)
{
    private readonly List<CombatGroup> _entities = new();

    private MapCell GetMapCell(Pos position)
    {
        TerrainType terrain = mapGenerator.CalculateTerrain(position);

        MapCell cell = new MapCell(terrain, position);
        return cell;
    }

    public MapCell[,] GetMapWindow(Pos upperLeftCorner, Pos lowerRightCorner)
    {
        if (upperLeftCorner.X > lowerRightCorner.X || upperLeftCorner.Y > lowerRightCorner.Y)
        {
            throw new ArgumentException("Upper left corner must be above and to the left of lower right corner");
        }

        MapCell[,] mapWindow = new MapCell[lowerRightCorner.X - upperLeftCorner.X + 1,
            lowerRightCorner.Y - upperLeftCorner.Y + 1];

        for (int y = upperLeftCorner.Y; y <= lowerRightCorner.Y; y++)
        {
            for (int x = upperLeftCorner.X; x <= lowerRightCorner.X; x++)
            {
                Pos pos = new Pos(x, y);
                MapCell cell = GetMapCell(pos);
                mapWindow[x - upperLeftCorner.X, y - upperLeftCorner.Y] = cell;
            }
        }

        return mapWindow;
    }

    public IEnumerable<CombatGroup> Entities => _entities;

    public void AddEntity(CombatGroup entity) => _entities.Add(entity);
    public void RemoveEntity(CombatGroup entity) => _entities.Remove(entity);

    public Pos GetOpenPositionNear(Pos centralPosition, int minDistance, int maxDistance)
    {
        Pos pos;
        do
        {
            int offset = random.Next(minDistance, maxDistance + 1);
            int xOffset = (int)Math.Round(random.NextDouble() * offset);
            int yOffset = offset - xOffset;

            // Randomly flip the offsets to negative
            if (random.NextDouble() < 0.5)
            {
                xOffset *= -1;
            }
            if (random.NextDouble() < 0.5)
            {
                yOffset *= -1;
            }

            pos = new Pos(centralPosition.X + xOffset, centralPosition.Y + yOffset);
        } while (_entities.Any(e => e.MapPos == pos));

        return pos;
    }
}