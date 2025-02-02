namespace EnigmaSimulator.Domain;

public class EnigmaMachine(Rotor[] rotors, Plugboard plugboard, Reflector reflector)
{
    private readonly List<IEnigmaVisitor> _observers = new();
    
    public char EncodeAndAdvance(char input)
    {
        Advance();
        
        char output = Encode(input);

        return output;
    }

    public void Register(IEnigmaVisitor observer)
    {
        _observers.Add(observer);
        for (int index = 0; index < rotors.Length; index++)
        {
            var rotor = rotors[index];
            rotor.Register(observer);
        }
        reflector.Register(observer);
        plugboard.Register(observer);
    }

    private void Advance()
    {
        Queue<Rotor> rotorsToAdvance = new(rotors.Reverse());
        bool shouldAdvance = true;
        while (shouldAdvance && rotorsToAdvance.Count > 0)
        {
            Rotor rotor = rotorsToAdvance.Dequeue();
            shouldAdvance = rotor.Advance();
        }
    }

    private char Encode(char input)
    {
        input = char.ToUpper(input);
        char output = plugboard.Encode(input);
        
        foreach (var rotor in rotors.Reverse())
        {
            output = rotor.Encode(output);
        }
        
        output = reflector.Encode(output);
        
        foreach (var rotor in rotors)
        {
            output = rotor.Encode(output);
        }
        output = plugboard.Encode(output);

        foreach (var observer in _observers)
        {
            observer.Complete(output);
        }
        return output;
    }
}