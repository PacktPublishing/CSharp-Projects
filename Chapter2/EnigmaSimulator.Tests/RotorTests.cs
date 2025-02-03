using EnigmaSimulator.Domain;
using Shouldly;

namespace EnigmaSimulator.Tests;

public class RotorTests
{
    [TestCase(1,2, false)]
    [TestCase(5,6, false)]
    [TestCase(17,18, true)] // Notch at Q on Enigma 1
    [TestCase(26,1, false)]
    public void RotorShouldAdvanceCorrectly(int initialPosition, int expectedPosition, bool expectedNotch)
    {
        // Arrange
        Rotor rotor = new(Rotor.Enigma1, initialPosition);
        
        // Act
        bool encounteredNotch = rotor.Advance();
        
        // Assert
        rotor.Position.ShouldBe(expectedPosition);
        encounteredNotch.ShouldBe(expectedNotch);
    }
    
    [TestCase('A', Rotor.Enigma1, 1, true, 'E')] // 1st letter of sequence
    [TestCase('a', Rotor.Enigma1, 1, true, 'E')]
    [TestCase('A', Rotor.Enigma1, 2, true, 'J')] // 2nd letter of sequence
    [TestCase('Z', Rotor.Enigma1, 1, true, 'J')]
    [TestCase('Z', Rotor.Enigma1, 2, true, 'D')] // Should wrap back to 1st
    [TestCase('A', Rotor.Enigma3, 1, true, 'B')]
    [TestCase('A', Rotor.Enigma3, 2, true, 'C')]
    [TestCase('D', Rotor.Enigma3, 1, true, 'H')]
    [TestCase('D', Rotor.Enigma3, 2, true, 'I')]
    [TestCase('G', Rotor.Enigma3, 1, true, 'C')]
    [TestCase('S', Rotor.Enigma3, 1, true, 'G')]
    [TestCase('X', Rotor.Enigma3, 2, true, 'P')]
    [TestCase('Z', Rotor.Enigma3, 1, true, 'O')]
    [TestCase('Z', Rotor.Enigma3, 2, true, 'A')]
    [TestCase('D', Rotor.Enigma3, 3, true, 'J')]
    [TestCase('J', Rotor.Enigma2, 1, true, 'B')]
    [TestCase('B', Rotor.Enigma1, 1, true, 'K')]
    [TestCase('N', Rotor.Enigma1, 1, false, 'K')]
    [TestCase('K', Rotor.Enigma2, 1, false, 'D')]
    [TestCase('D', Rotor.Enigma3, 3, false, 'A')]
    [TestCase('S', Rotor.Enigma1, 1, false, 'S')]
    [TestCase('S', Rotor.Enigma2, 1, false, 'E')]
    [TestCase('E', Rotor.Enigma3, 1, false, 'P')]
    public void RotorMappingTests(char input, string mapping, int position, bool isForward, char expected)
    {
        // Arrange
        Rotor rotor = new(mapping, position);
        
        // Act
        char output = rotor.Encode(input, isForward);
        
        // Assert
        output.ShouldBe(expected);
    }
}