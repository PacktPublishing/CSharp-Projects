using EnigmaSimulator.Domain;

namespace EnigmaSimulator.Tests.Helpers;

public class CapturingReflector(string inputMapping) : Reflector(inputMapping)
{
    public override char Encode(char input, bool isForward)
    {
        LastInput = input;
        char output = base.Encode(input, isForward);
        LastOutput = output;
        
        return output;
    }

    public char LastOutput { get; private set; }

    public char LastInput { get; private set; }
}