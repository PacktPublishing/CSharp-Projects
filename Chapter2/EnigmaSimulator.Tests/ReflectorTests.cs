using EnigmaSimulator.Domain;
using Shouldly;

namespace EnigmaSimulator.Tests;

public class ReflectorTests
{
    [TestCase('A', 'Y')]
    [TestCase('J', 'X')]
    [TestCase('X', 'J')]
    [TestCase('F', 'S')]
    public void ReflectorShouldMapInputCorrectly(char input, char expected)
    {
        // Arrange
        Reflector reflector = new(Reflector.ReflectorB);
        
        // Act
        char output = reflector.Encode(input, isForward: true);
        
        // Assert
        output.ShouldBe(expected);
    }
}