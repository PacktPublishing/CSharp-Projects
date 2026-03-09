using ConsoleRolePlayingGame.Overworld.Structure;

namespace ConsoleRolePlayingGame.Overworld.Generators;

public class MapGenerator
{
    private readonly PerlinNoiseProvider _heightNoise = new(seed: 1234);
    private readonly PerlinNoiseProvider _tempNoise = new(seed: 5678);
    
    public TerrainType CalculateTerrain(Pos pos)
    {
        float height = _heightNoise.Generate(pos.X, pos.Y);
        float temp = _tempNoise.Generate(pos.X, pos.Y);

        return height switch
        {
            < 0.15f => TerrainType.DeepWater,
            < 0.35f => TerrainType.Water,
            < 0.4f => TerrainType.Desert,
            > 0.8f => TerrainType.Mountain,
            
            _ => temp switch
            {
                < 0.4f => TerrainType.Forest,
                > 0.8f => TerrainType.Desert,
                _ => TerrainType.Grass
            }
        };
    }
}