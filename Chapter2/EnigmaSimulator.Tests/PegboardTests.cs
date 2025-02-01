using EnigmaSimulator.Domain;
using Shouldly;

namespace EnigmaSimulator.Tests;

public class PegboardTests
{
    [Test]
    public void ConnectionPresentAfterBeingConfigured()
    {
        // Arrange
        Pegboard pegboard = new("HI");
        
        // Act
        char output = pegboard.Encode('H');
        
        // Assert
        output.ShouldBe('I');
    }
    
    [TestCase('N')]
    [TestCase('E')]
    [TestCase('T')]
    public void PegboardShouldReturnInputWhenNotConnected(char input)
    {
        // Arrange
        Pegboard pegboard = new();
        
        // Act
        char output = pegboard.Encode(input);
        
        // Assert
        output.ShouldBe(input);
    }
    
    [Test]
    public void DuplicateConnectionsAreNotAllowed()
    {
        Should.Throw<ArgumentException>(
            () => new Pegboard("HI", "ID")
        );
    }
}