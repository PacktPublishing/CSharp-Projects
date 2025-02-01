using EnigmaSimulator.Domain;
using Shouldly;

namespace EnigmaSimulator.Tests;

public class ReflectorTests
{
    [Test]
    public void ReflectorShouldMapInputCorrectly()
    {
        // Arrange
        Reflector reflector = new(Reflector.ReflectorB);
        
        // Act
        char output = reflector.Encode('A');
        
        // Assert
        output.ShouldBe('Y');
    }
}