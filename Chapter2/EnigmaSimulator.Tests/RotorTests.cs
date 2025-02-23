using EnigmaSimulator.Domain;
using Shouldly;

namespace EnigmaSimulator.Tests;

public class RotorTests
{
    [Theory]
    [InlineData(1,2, false)]
    [InlineData(5,6, false)]
    [InlineData(17,18, true)] // Notch at Q on Enigma 1
    [InlineData(26,1, false)]
    public void RotorShouldAdvance(int position, int expectedPosition, bool expectNotch)
    {
        // Arrange
        Rotor rotor = new(RotorSets.Enigma1, position);
        
        // Act
        bool encounteredNotch = rotor.Advance();
        
        // Assert
        rotor.Position.ShouldBe(expectedPosition);
        encounteredNotch.ShouldBe(expectNotch);
    }
    
    [Theory]
    [InlineData('A', RotorSets.Enigma1, 1, true, 'E')] // 1st letter of sequence
    [InlineData('A', RotorSets.Enigma1, 2, true, 'J')] // 2nd letter of sequence
    [InlineData('Z', RotorSets.Enigma1, 1, true, 'J')]
    [InlineData('Z', RotorSets.Enigma1, 2, true, 'D')] // Should wrap back to 1st
    [InlineData('A', RotorSets.Enigma3, 1, true, 'B')]
    [InlineData('A', RotorSets.Enigma3, 2, true, 'C')]
    [InlineData('D', RotorSets.Enigma3, 1, true, 'H')]
    [InlineData('D', RotorSets.Enigma3, 2, true, 'I')]
    [InlineData('G', RotorSets.Enigma3, 1, true, 'C')]
    [InlineData('S', RotorSets.Enigma3, 1, true, 'G')]
    [InlineData('X', RotorSets.Enigma3, 2, true, 'P')]
    [InlineData('Z', RotorSets.Enigma3, 1, true, 'O')]
    [InlineData('Z', RotorSets.Enigma3, 2, true, 'A')]
    [InlineData('D', RotorSets.Enigma3, 3, true, 'J')]
    [InlineData('J', RotorSets.Enigma2, 1, true, 'B')]
    [InlineData('B', RotorSets.Enigma1, 1, true, 'K')]
    [InlineData('N', RotorSets.Enigma1, 1, false, 'K')]
    [InlineData('K', RotorSets.Enigma2, 1, false, 'D')]
    [InlineData('D', RotorSets.Enigma3, 3, false, 'A')]
    [InlineData('S', RotorSets.Enigma1, 1, false, 'S')]
    [InlineData('S', RotorSets.Enigma2, 1, false, 'E')]
    [InlineData('E', RotorSets.Enigma3, 1, false, 'P')]
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