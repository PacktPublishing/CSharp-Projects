using System.Collections;

namespace ConsoleRolePlayingGame.Domain.Overworld;

public class WorldMap(MapGenerator mapGenerator)
{
    private readonly List<IMapEntity> _entities = new();

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
        
        MapCell[,] mapWindow = new MapCell[lowerRightCorner.X - upperLeftCorner.X + 1, lowerRightCorner.Y - upperLeftCorner.Y + 1];
        
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

    public IEnumerable<IMapEntity> Entities => _entities;
    
    public void AddEntity(IMapEntity entity) => _entities.Add(entity);
    public void RemoveEntity(IMapEntity entity) => _entities.Remove(entity);
}