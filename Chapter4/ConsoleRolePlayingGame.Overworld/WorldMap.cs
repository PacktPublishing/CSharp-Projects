using ConsoleRolePlayingGame.Overworld.Entities;
using ConsoleRolePlayingGame.Overworld.Generators;
using ConsoleRolePlayingGame.Overworld.Structure;

namespace ConsoleRolePlayingGame.Overworld;

public class WorldMap(MapGenerator mapGenerator, Random random)
{
    public MapCell[,] GetMapWindow(Pos topLeft, int width, int height)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(width, 0);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(height, 0);

        MapCell[,] mapWindow = new MapCell[width, height];

        for (int y = topLeft.Y; y < topLeft.Y + height; y++)
        {
            for (int x = topLeft.X; x < topLeft.X + width; x++)
            {
                Pos pos = new(x, y);
                TerrainType terrain = mapGenerator.CalculateTerrain(pos);
                mapWindow[x - topLeft.X, y - topLeft.Y] = new MapCell(terrain, pos);
            }
        }

        return mapWindow;
    }

    public IEnumerable<IMapEntity> Entities => _entities.AsReadOnly();
    private readonly List<IMapEntity> _entities = new();
    
    public void AddEntity(IMapEntity entity) => _entities.Add(entity);
    public void RemoveEntity(IMapEntity entity) => _entities.Remove(entity);
}