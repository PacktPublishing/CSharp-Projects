using EnigmaSimulator.Domain;
using Shouldly;

namespace EnigmaSimulator.Tests;

public class RotorTests
{
    [TestCase('A', 1, 'E')] // 1st letter of sequence
    [TestCase('a', 1, 'E')]
    [TestCase('A', 2, 'J')] // 2nd letter of sequence
    [TestCase('Z', 1, 'J')]
    [TestCase('Z', 2, 'D')] // Should wrap back to 1st
    public void RotorShouldMapInputCorrectly(char input, int position, char expectedOutput)
    {
        // Arrange
        Rotor rotor = new(Rotor.Enigma1, position);
        
        // Act
        char output = rotor.Encode(input);
        
        // Assert
        output.ShouldBe(expectedOutput);
    }
    
    [TestCase(1,2)]
    [TestCase(5,6)]
    [TestCase(26,1)]
    public void RotorShouldAdvanceCorrectly(int initialPosition, int expectedPosition)
    {
        // Arrange
        Rotor rotor = new(Rotor.Enigma1, initialPosition);
        
        // Act
        rotor.Advance();
        
        // Assert
        rotor.Position.ShouldBe(expectedPosition);
    }
    
    [TestCase('A', 1, 'B')]
    [TestCase('A', 2, 'C')]
    [TestCase('D', 1, 'H')]
    [TestCase('D', 2, 'I')]
    [TestCase('G', 1, 'C')]
    [TestCase('S', 1, 'G')]
    [TestCase('X', 2, 'P')]
    [TestCase('Z', 1, 'O')]
    [TestCase('Z', 2, 'A')]
    public void Rotor3MappingTests(char input, int position, char expected)
    {
        // Arrange
        Rotor rotor = new(Rotor.Enigma3, position);
        
        // Act
        char output = rotor.Encode(input);
        
        // Assert
        output.ShouldBe(expected);
    }

    [TestCase('A', 1, 'A')]
    [TestCase('A', 2, 'B')]
    [TestCase('Z', 1, 'Z')]
    [TestCase('Z', 2, 'A')]
    public void PositionAndRingTests(char input, int position, char expected)
    {
        // Arrange
        Rotor rotor = new(Rotor.Enigma1, position);
        
        // Act
        char output = rotor.AdjustForPositionAndRing(input, true);
        
        // Assert
        output.ShouldBe(expected);
    }
}