namespace EnigmaSimulator.Domain;

public class EnigmaMachine
{
    private readonly Reflector _reflector;
    private readonly Pegboard _pegboard;
    private readonly Rotor[] _rotors;
    
    public EnigmaMachine(Rotor[] rotors, Pegboard pegboard, Reflector reflector)
    {
        _reflector = reflector;
        _pegboard = pegboard;
        _rotors = rotors;
    }
    
    public char Encode(char input)
    {
        char output = _pegboard.Encode(input);
        
        foreach (var rotor in _rotors)
        {
            output = rotor.Encode(output);
        }
        
        output = _reflector.Encode(output);
        
        foreach (var rotor in _rotors.Reverse())
        {
            output = rotor.Encode(output);
        }

        return output;
    }
}