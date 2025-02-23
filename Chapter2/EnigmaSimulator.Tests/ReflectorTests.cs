using EnigmaSimulator.Domain;
using Shouldly;

namespace EnigmaSimulator.Tests;

public class ReflectorTests
{
    [Theory]
    [InlineData('A', 'Y')]
    [InlineData('J', 'X')]
    [InlineData('X', 'J')]
    [InlineData('F', 'S')]
    public void ReflectorShouldMapInputCorrectly(char input, char expected)
    {
        // Arrange
        Reflector reflector = new(ReflectorSets.ReflectorB);
        
        // Act
        char output = reflector.Encode(input);
        
        // Assert
        output.ShouldBe(expected);
    }
}