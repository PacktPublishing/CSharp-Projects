using ConsoleRolePlayingGame.Overworld.Generators;
using Shouldly;

namespace ConsoleRolePlayingGame.Overworld.Tests;

public class PerlinNoiseProviderTests
{
    [Theory]
    [InlineData(1234, 1, 1, 0.8184)]
    [InlineData(1234, 1, -1, 0.8185)]
    [InlineData(5678, 1, -1, 0.3938)]    
    [InlineData(1234, 8, 0, 0.332)]
    [InlineData(5678, 8, 0, 0.1325)]
    public void ProducesDeterministicResults(int seed, int x, int y, float expected)
    {
        // Arrange
        PerlinNoiseProvider noise = new(seed, scale: 0.1f);
        
        // Act
        float result = noise.Generate(x, y);
        
        // Assert
        result.ShouldBe(expected, tolerance: 0.001);
    }
}