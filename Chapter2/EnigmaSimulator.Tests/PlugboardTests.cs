using EnigmaSimulator.Domain;
using Shouldly;

namespace EnigmaSimulator.Tests;

public class PlugboardTests
{
    [Fact]
    public void ConnectionPresentAfterBeingConfigured()
    {
        // Arrange
        Plugboard plugboard = new("HI");
        
        // Act
        char output = plugboard.Encode('H', isForward:true);
        
        // Assert
        output.ShouldBe('I');
    }
    
    [Theory]
    [InlineData('N')]
    [InlineData('E')]
    [InlineData('T')]
    public void PegboardShouldReturnInputWhenNotConnected(char input)
    {
        // Arrange
        Plugboard plugboard = new();
        
        // Act
        char output = plugboard.Encode(input, isForward: true);
        
        // Assert
        output.ShouldBe(input);
    }
    
    [Fact]
    public void DuplicateConnectionsAreNotAllowed()
    {
        Should.Throw<ArgumentException>(
            () => new Plugboard("HI", "ID")
        );
    }
}