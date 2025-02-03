using EnigmaSimulator.Domain;
using EnigmaSimulator.Domain.Utilities;
using Shouldly;

namespace EnigmaSimulator.Tests;

public class BidirectionalCharEncoderTests
{
    [TestCase('A', true, 'E')]
    [TestCase('E', false, 'A')]
    public void ShouldEncodeBidirectionally(char input, bool isForward, char expected)
    {
        // Arrange
        BidirectionalCharEncoder encoder = BidirectionalCharEncoder.FromMapping(Rotor.Enigma1);
        
        // Act
        char output = encoder.Encode(input, isForward);
        
        // Assert
        output.ShouldBe(expected);
    }
}