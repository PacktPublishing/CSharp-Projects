using ConsoleRolePlayingGame.Overworld;
using Shouldly;

namespace ConsoleRolePlayingGame.Tests;

public class TerrainProviderTests
{
    [Theory]
    [InlineData(8, 0, TerrainType.Grass)]
    [InlineData(0, -2, TerrainType.Desert)]
    [InlineData(11, -1, TerrainType.DeepWater)]
    [InlineData(10, -1, TerrainType.Water)]
    public void TerrainGenerationShouldBeDeterministic(int x, int y, TerrainType expected)
    {
        // Arrange
        MapGenerator generator = new();
        Pos pos = new(x, y);
        
        // Act
        TerrainType actual = generator.CalculateTerrain(pos);
        
        // Assert
        actual.ShouldBe(expected);
    }
}