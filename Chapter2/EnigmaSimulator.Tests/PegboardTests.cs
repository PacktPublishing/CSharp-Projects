using EnigmaSimulator.Domain;
using Shouldly;

namespace EnigmaSimulator.Tests;

public class PegboardTests
{
    
    [Test]
    public void ConnectionPresentAfterBeingConfigured()
    {
        // Arrange
        Pegboard pegboard = new();
        pegboard.Connect('H', 'I');
        
        // Act
        bool hasConnection = pegboard.HasConnection('H');
        
        // Assert
        hasConnection.ShouldBeTrue();
    }
    
    [Test]
    public void ConfiguredConnectionsAreMapped()
    {
        // Arrange
        Pegboard pegboard = new();
        pegboard.Connect('H', 'I');
        
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
}