using EnigmaSimulator.Domain;
using Shouldly;

namespace EnigmaSimulator.Tests;

public class PlugboardTests
{
    [Test]
    public void ConnectionPresentAfterBeingConfigured()
    {
        // Arrange
        Plugboard plugboard = new("HI");
        
        // Act
        char output = plugboard.Encode('H');
        
        // Assert
        output.ShouldBe('I');
    }
    
    [TestCase('N')]
    [TestCase('E')]
    [TestCase('T')]
    public void PegboardShouldReturnInputWhenNotConnected(char input)
    {
        // Arrange
        Plugboard plugboard = new();
        
        // Act
        char output = plugboard.Encode(input);
        
        // Assert
        output.ShouldBe(input);
    }
    
    [Test]
    public void DuplicateConnectionsAreNotAllowed()
    {
        Should.Throw<ArgumentException>(
            () => new Plugboard("HI", "ID")
        );
    }
}