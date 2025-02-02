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
                new Rotor(Rotor.Enigma3, position: 2),
            ],
            new Plugboard(),
            new Reflector(Reflector.ReflectorB)
        );
        
        // Act
        char output = enigma.Encode(input);
        
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
                new Rotor(Rotor.Enigma3, position: 2),
            ],
            new Plugboard(),
            reflector
        );
        
        // Act
        _ = enigma.Encode(input);
        
        // Assert
        reflector.LastInput.ShouldBe(expected);
    } 
}