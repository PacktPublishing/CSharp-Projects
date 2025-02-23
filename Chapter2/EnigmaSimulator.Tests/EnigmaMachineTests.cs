using EnigmaSimulator.Domain;
using EnigmaSimulator.Tests.Helpers;
using Shouldly;

namespace EnigmaSimulator.Tests;

public class EnigmaMachineTests
{
    [Theory]
    [InlineData('A', 'B')]
    [InlineData('G', 'X')]
    [InlineData('X', 'G')]
    [InlineData('Z', 'U')]
    public void EnigmaShouldProduceCorrectOutput(char input, char expected)
    {
        // Arrange
        EnigmaMachine enigma = new(new Plugboard(),
            new Rotor(RotorSets.Enigma3),
            new Rotor(RotorSets.Enigma2),
            new Rotor(RotorSets.Enigma1),
            new Reflector(ReflectorSets.ReflectorB));

        // Act
        char output = enigma.Encode(input);

        // Assert
        output.ShouldBe(expected);
    }

    [Theory]
    [InlineData('Z', 'E')]
    public void EnigmaShouldReachReflectorWithCorrectOutputForFirstKeystroke(char input, char expected)
    {
        // Arrange
        CapturingReflector reflector = new(ReflectorSets.ReflectorB);
        EnigmaMachine enigma = new(new Plugboard(),
            new Rotor(RotorSets.Enigma3),
            new Rotor(RotorSets.Enigma2),
            new Rotor(RotorSets.Enigma1),
            reflector);

        // Act
        _ = enigma.Encode(input);

        // Assert
        reflector.LastInput.ShouldBe(expected);
    }

    [Theory]
    [InlineData("HELLO", "ILBDA")]
    [InlineData("ILBDA", "HELLO")]
    [InlineData("THEENIGMAMACHINEISENCODINGPROPERLY", "OPCWCLZNLVKKGQONYNOZVDFUSNKXJGUJOZ")]
    [InlineData("OPCWCLZNLVKKGQONYNOZVDFUSNKXJGUJOZ", "THEENIGMAMACHINEISENCODINGPROPERLY")]
    public void EnigmaShouldEncodeStringsCorrectly(string input, string expected)
    {
        // Arrange
        EnigmaMachine enigma = new(new Plugboard(),
            new Rotor(RotorSets.Enigma3),
            new Rotor(RotorSets.Enigma2),
            new Rotor(RotorSets.Enigma1),
            new Reflector(ReflectorSets.ReflectorB));

        // Act
        string output = enigma.Encode(input);

        // Assert
        output.ShouldBe(expected);
    }


    [Theory]
    [InlineData("HELLO", "IQBDA")]
    [InlineData("THEENIGMAMACHINEISENCODINGPROPERLY", "GPOSRLZELVKKGQSNYEYPVDFUCEKTJGFJOZ")]
    public void EnigmaShouldEncodeStringsCorrectlyWithPlugboard(string input, string expected)
    {
        // Arrange
        EnigmaMachine enigma = new(new Plugboard("NE", "XT"),
            new Rotor(RotorSets.Enigma3),
            new Rotor(RotorSets.Enigma2),
            new Rotor(RotorSets.Enigma1),
            new Reflector(ReflectorSets.ReflectorB));

        // Act
        string output = enigma.Encode(input);

        // Assert
        output.ShouldBe(expected);
    }
}