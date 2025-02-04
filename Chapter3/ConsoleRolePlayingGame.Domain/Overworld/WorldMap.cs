namespace ConsoleRolePlayingGame.Domain.Overworld;

public class WorldMap
{
    public WorldMap(int width, int height, Random rand)
    {
        Width = width;
        Height = height;
                
        Map = new MapCell[width, height];
        
        // Define the parameters for the Gaussian distribution
        double mean = 10.0; 
        double stdDev = 5.0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                double cellHeight = Math.Round(GetGaussianRandom(rand, mean, stdDev));

                TerrainType terrain = cellHeight switch
                {
                    <= 5 => TerrainType.Water,
                    <= 6 => TerrainType.Desert,
                    >= 13 => TerrainType.Mountain,
                    _ => TerrainType.Grass
                };

                MapCell cell = new MapCell(terrain, new Pos(x, y), height);
                Map[x, y] = cell;
            }
        }
        
        PlayerPos = new Pos((int)Math.Floor(width / 2.0), (int)Math.Floor(height / 2.0));
    }

    public Pos PlayerPos { get; set; }

    private static double GetGaussianRandom(Random rand, double mean, double stdDev)
    {
        double u1 = 1.0 - rand.NextDouble();
        double u2 = 1.0 - rand.NextDouble();
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        return mean + stdDev * randStdNormal;
    }

    public MapCell[,] Map { get; }
    
    public int Width { get; }
    public int Height { get; }
}