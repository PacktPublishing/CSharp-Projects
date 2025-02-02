using EnigmaSimulator.Domain;

namespace EnigmaSimulator.Tests;

public class CapturingReflector(string inputMapping) : Reflector(inputMapping)
{
    public override char Encode(char input)
    {
        LastInput = input;
        char output = base.Encode(input);
        LastOutput = output;
        
        return output;
    }

    public char LastOutput { get; private set; }

    public char LastInput { get; private set; }
}