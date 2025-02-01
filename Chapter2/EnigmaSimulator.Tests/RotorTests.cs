using EnigmaSimulator.Domain;
using Shouldly;

namespace EnigmaSimulator.Tests;

public class RotorTests
{
    [TestCase('A', 0, 'E')] // 1st letter of sequence
    [TestCase('a', 0, 'E')]
    [TestCase('A', 1, 'K')] // 2nd letter of sequence
    [TestCase('Z', 0, 'J')]
    [TestCase('Z', 1, 'E')] // Should wrap back to 1st
    public void RotorShouldMapInputCorrectly(char input, int position, char expectedOutput)
    {
        // Arrange
        Rotor rotor = new(Rotor.Enigma1, position);
        
        // Act
        char output = rotor.Encode(input);
        
        // Assert
        output.ShouldBe(expectedOutput);
    }
    
    [TestCase(0,1)]
    [TestCase(5,6)]
    [TestCase(25,0)]
    public void RotorShouldAdvanceCorrectly(int initialPosition, int expectedPosition)
    {
        // Arrange
        Rotor rotor = new(Rotor.Enigma1, initialPosition);
        
        // Act
        rotor.Advance();
        
        // Assert
        rotor.Position.ShouldBe(expectedPosition);
    }
}