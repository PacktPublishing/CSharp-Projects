using SimplexNoise;

namespace ConsoleRolePlayingGame.Overworld.Generators;

public class PerlinNoiseProvider(int seed, float scale = 0.05f)
{
    private static readonly Lock _lock = new();
    
    public float Generate(int x, int y)
    {
        float result;
        
        lock (_lock)
        {
            Noise.Seed = seed;
            result = Noise.CalcPixel2D(x, y, scale);
        }

        const float maxGeneratedValue = 256f;
        return result / maxGeneratedValue;
    }
}