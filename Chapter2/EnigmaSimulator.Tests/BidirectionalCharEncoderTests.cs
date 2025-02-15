using EnigmaSimulator.Domain;
using EnigmaSimulator.Domain.Utilities;
using Shouldly;

namespace EnigmaSimulator.Tests;

public class BidirectionalCharEncoderTests
{
    [Theory]
    [InlineData('A', true, 0, 'E')]
    [InlineData('E', false, 0, 'A')]    
    [InlineData('A', true, 1, 'J')]
    [InlineData('E', false, 1, 'C')]
    [InlineData('A', true, 25, 'K')]
    [InlineData('E', false, 25, 'H')]
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