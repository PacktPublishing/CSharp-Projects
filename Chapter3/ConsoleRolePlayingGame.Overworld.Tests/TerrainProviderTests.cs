using ConsoleRolePlayingGame.Overworld.Generators;
using ConsoleRolePlayingGame.Overworld.Structure;
using Shouldly;

namespace ConsoleRolePlayingGame.Overworld.Tests;

public class TerrainProviderTests
{
    [Theory]
    [InlineData(8, 0, TerrainType.Grass)]
    [InlineData(0, -2, TerrainType.Desert)]
    [InlineData(11, -1, TerrainType.DeepWater)]
    [InlineData(10, -1, TerrainType.Water)]
    public void ProducesExpectedResult(int x, int y, TerrainType expected)
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