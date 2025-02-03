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
        Rotor rotor = new(RotorSets.Enigma1, initialPosition);
        
        // Act
        bool encounteredNotch = rotor.Advance();
        
        // Assert
        rotor.Position.ShouldBe(expectedPosition);
        encounteredNotch.ShouldBe(expectedNotch);
    }
    
    [TestCase('A', RotorSets.Enigma1, 1, true, 'E')] // 1st letter of sequence
    [TestCase('a', RotorSets.Enigma1, 1, true, 'E')]
    [TestCase('A', RotorSets.Enigma1, 2, true, 'J')] // 2nd letter of sequence
    [TestCase('Z', RotorSets.Enigma1, 1, true, 'J')]
    [TestCase('Z', RotorSets.Enigma1, 2, true, 'D')] // Should wrap back to 1st
    [TestCase('A', RotorSets.Enigma3, 1, true, 'B')]
    [TestCase('A', RotorSets.Enigma3, 2, true, 'C')]
    [TestCase('D', RotorSets.Enigma3, 1, true, 'H')]
    [TestCase('D', RotorSets.Enigma3, 2, true, 'I')]
    [TestCase('G', RotorSets.Enigma3, 1, true, 'C')]
    [TestCase('S', RotorSets.Enigma3, 1, true, 'G')]
    [TestCase('X', RotorSets.Enigma3, 2, true, 'P')]
    [TestCase('Z', RotorSets.Enigma3, 1, true, 'O')]
    [TestCase('Z', RotorSets.Enigma3, 2, true, 'A')]
    [TestCase('D', RotorSets.Enigma3, 3, true, 'J')]
    [TestCase('J', RotorSets.Enigma2, 1, true, 'B')]
    [TestCase('B', RotorSets.Enigma1, 1, true, 'K')]
    [TestCase('N', RotorSets.Enigma1, 1, false, 'K')]
    [TestCase('K', RotorSets.Enigma2, 1, false, 'D')]
    [TestCase('D', RotorSets.Enigma3, 3, false, 'A')]
    [TestCase('S', RotorSets.Enigma1, 1, false, 'S')]
    [TestCase('S', RotorSets.Enigma2, 1, false, 'E')]
    [TestCase('E', RotorSets.Enigma3, 1, false, 'P')]
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