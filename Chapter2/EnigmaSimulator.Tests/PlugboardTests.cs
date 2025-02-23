using EnigmaSimulator.Domain;
using Shouldly;

namespace EnigmaSimulator.Tests;

public class PlugboardTests
{
    [Theory]
    [InlineData('H', 'O')]
    [InlineData('O', 'H')]
    [InlineData('A', 'W')]
    [InlineData('X', 'X')]
    public void ConnectionPresentAfterBeingConfigured(char input, char expected)
    {
        // Arrange
        Plugboard plugboard = new("OH", "WA");
        
        // Act
        char output = plugboard.Encode(input);
        
        // Assert
        output.ShouldBe(expected);
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
        char output = plugboard.Encode(input);
        
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