using EnigmaSimulator.Domain;
using Shouldly;

namespace EnigmaSimulator.Tests;

public class RotorTests
{
    [TestCase('A', 'E')]
    [TestCase('a', 'E')]
    [TestCase('Z', 'J')]
    public void RotorShouldMapInputCorrectly(char input, char expectedOutput)
    {
        // Arrange
        Rotor rotor = new(Rotor.Enigma1);
        
        // Act
        char output = rotor.Encode(input);
        
        // Assert
        output.ShouldBe(expectedOutput);
    }
}