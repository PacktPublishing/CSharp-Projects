namespace ConsoleRolePlayingGame.Domain.Overworld;

public class WorldMap
{
    private readonly MapGenerator _mapGenerator = new();
    
    public WorldMap(int width, int height, Random rand)
    {
        Width = width;
        Height = height;
                
        Map = new MapCell[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Pos position = new Pos(x, y);
                TerrainType terrain = _mapGenerator.CalculateTerrain(position);

                MapCell cell = new MapCell(terrain, position, height);
                Map[x, y] = cell;
            }
        }
        
        PlayerPos = new Pos((int)Math.Floor(width / 2.0), (int)Math.Floor(height / 2.0));
    }

    public Pos PlayerPos { get; set; }

    public MapCell[,] Map { get; }
    
    public int Width { get; }
    public int Height { get; }
}