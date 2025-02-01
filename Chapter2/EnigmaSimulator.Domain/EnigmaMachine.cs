namespace EnigmaSimulator.Domain;

public class EnigmaMachine(Rotor[] rotors, Pegboard pegboard, Reflector reflector)
{
    public char EncodeAndAdvance(char input)
    {
        char output = Encode(input);
        Advance();

        return output;
    }

    private void Advance()
    {
        Queue<Rotor> rotorsToAdvance = new(rotors);
        bool shouldAdvance = true;
        while (shouldAdvance && rotorsToAdvance.Count > 0)
        {
            Rotor rotor = rotorsToAdvance.Dequeue();
            shouldAdvance = rotor.Advance();
        }
    }

    private char Encode(char input)
    {
        char output = pegboard.Encode(input);
        
        foreach (var rotor in rotors)
        {
            output = rotor.Encode(output);
        }
        
        output = reflector.Encode(output);
        
        foreach (var rotor in rotors.Reverse())
        {
            output = rotor.Encode(output);
        }

        return output;
    }
}