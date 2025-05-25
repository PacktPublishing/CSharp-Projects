using ConsoleRolePlayingGame.Overworld.Structure;

namespace ConsoleRolePlayingGame.Overworld.Generators;

public class MapGenerator
{
    private readonly PerlinNoiseProvider _heightNoise = new(1234);
    private readonly PerlinNoiseProvider _temperatureNoise = new(5678);
    
    public TerrainType CalculateTerrain(Pos position)
    {
        float height = _heightNoise.Generate(position.X, position.Y);
        float temperature = _temperatureNoise.Generate(position.X, position.Y);

        return height switch
        {
            < 0.15f => TerrainType.DeepWater,
            < 0.35f => TerrainType.Water,
            < 0.4f => TerrainType.Desert,
            > 0.8f => TerrainType.Mountain,
            
            _ => temperature switch
            {
                < 0.4f => TerrainType.Forest,
                > 0.8f => TerrainType.Desert,
                _ => TerrainType.Grass
            }
        };
    }
}