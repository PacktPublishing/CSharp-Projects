using EnigmaSimulator.Domain;
using Shouldly;

namespace EnigmaSimulator.Tests;

public class EnigmaMachineTests
{
    [TestCase('A', 'B')]
    [TestCase('G', 'X')]
    [TestCase('X', 'G')]
    [TestCase('Z', 'U')]
    public void EnigmaShouldProduceCorrectOutputForFirstKeystroke(char input, char expected)
    {
        // Arrange
        EnigmaMachine enigma = new(
            [
                new Rotor(Rotor.Enigma1),
                new Rotor(Rotor.Enigma2),
                new Rotor(Rotor.Enigma3),
            ],
            new Plugboard(),
            new Reflector(Reflector.ReflectorB)
        );
        
        // Act
        char output = enigma.AdvanceAndEncode(input);
        
        // Assert
        output.ShouldBe(expected);
    } 
    
    [TestCase('Z', 'E')]
    public void EnigmaShouldReachReflectorWithCorrectOutputForFirstKeystroke(char input, char expected)
    {
        // Arrange
        CapturingReflector reflector = new(Reflector.ReflectorB);
        EnigmaMachine enigma = new(
            [
                new Rotor(Rotor.Enigma1),
                new Rotor(Rotor.Enigma2),
                new Rotor(Rotor.Enigma3),
            ],
            new Plugboard(),
            reflector
        );
        
        // Act
        _ = enigma.AdvanceAndEncode(input);
        
        // Assert
        reflector.LastInput.ShouldBe(expected);
    }
    
    [TestCase("HELLO", "ILBDA")]
    [TestCase("THEENIGMAMACHINEISENCODINGPROPERLY", "OPCWCLZNLVKKGQONYNOZVDFUSNKXJGUJOZ")]
    public void EnigmaShouldEncodeStringsCorrectly(string input, string expected)
    {
        // Arrange
        EnigmaMachine enigma = new(
            [
                new Rotor(Rotor.Enigma1),
                new Rotor(Rotor.Enigma2),
                new Rotor(Rotor.Enigma3),
            ],
            new Plugboard(),
            new Reflector(Reflector.ReflectorB)
        );
        
        // Act
        string output = enigma.Encode(input);
        
        // Assert
        output.ShouldBe(expected);
    }
    
        
    [TestCase("HELLO", "IQBDA")]
    [TestCase("THEENIGMAMACHINEISENCODINGPROPERLY", "GPOSRLZELVKKGQSNYEYPVDFUCEKTJGFJOZ")]
    public void EnigmaShouldEncodeStringsCorrectlyWithPlugboard(string input, string expected)
    {
        // Arrange
        EnigmaMachine enigma = new(
            [
                new Rotor(Rotor.Enigma1),
                new Rotor(Rotor.Enigma2),
                new Rotor(Rotor.Enigma3),
            ],
            new Plugboard("NE", "XT"),
            new Reflector(Reflector.ReflectorB)
        );
        
        // Act
        string output = enigma.Encode(input);
        
        // Assert
        output.ShouldBe(expected);
    }
}