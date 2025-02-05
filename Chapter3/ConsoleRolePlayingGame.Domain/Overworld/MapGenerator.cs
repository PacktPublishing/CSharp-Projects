using SimplexNoise;

namespace ConsoleRolePlayingGame.Domain.Overworld;

public class MapGenerator
{
    private readonly int _heightSeed = 1234;
    private readonly int _temperatureSeed = 5678;
    
    public TerrainType CalculateTerrain(Pos position)
    {
        Noise.Seed = _heightSeed;
        float height = Noise.CalcPixel2D(position.X, position.Y, 0.1f) / 256.0f;
        
        Noise.Seed = _temperatureSeed;
        float temperature = Noise.CalcPixel2D(position.X, position.Y, 0.1f) / 256.0f;
        
        if (height < 0.3f)
        {
            return TerrainType.Water;
        }
        if (height < 0.4f || temperature > 0.8f )
        {
            return TerrainType.Desert;
        }
        if (height > 0.8f)
        {
            return TerrainType.Mountain;
        }
        if (temperature < 0.4f)
        {
            return TerrainType.Forest;
        }
        return TerrainType.Grass;
    }
}