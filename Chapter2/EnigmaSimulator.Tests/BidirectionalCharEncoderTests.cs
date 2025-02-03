using EnigmaSimulator.Domain;
using EnigmaSimulator.Domain.Utilities;
using Shouldly;

namespace EnigmaSimulator.Tests;

public class BidirectionalCharEncoderTests
{
    [TestCase('A', true, 0, 'E')]
    [TestCase('E', false, 0, 'A')]    
    [TestCase('A', true, 1, 'J')]
    [TestCase('E', false, 1, 'C')]
    [TestCase('A', true, 25, 'K')]
    [TestCase('E', false, 25, 'H')]
    public void ShouldEncodeBidirectionally(char input, bool isForward, int offset, char expected)
    {
        // Arrange
        string mapping = RotorSets.Enigma1[..26]; // Ignore the notch dash notation
        BidirectionalCharEncoder encoder = new(mapping);
        
        // Act
        char output = encoder.Encode(input, isForward, offset);
        
        // Assert
        output.ShouldBe(expected);
    }
}