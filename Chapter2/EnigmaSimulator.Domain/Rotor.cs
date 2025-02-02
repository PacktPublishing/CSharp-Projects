using EnigmaSimulator.Domain.Utilities;

namespace EnigmaSimulator.Domain;

public class Rotor
{
    private readonly List<IEnigmaVisitor> _observers = [];
    public const int AlphabetLength = 26;
    
    public const string Enigma1 = "EKMFLGDQVZNTOWYHXUSPAIBRCJ";
    public const string Enigma2 = "AJDKSIRUXBLHWTMCQGZNPYFVOE";
    public const string Enigma3 = "BDFHJLCPRTXVZNYEIWGAKMUSQO";
    public const string Enigma4 = "ESOVPZJAYQUIRHXLNFTGKDCMWB";
    public const string Enigma5 = "VZBRGITYUPSDNHLXAWMJQOFECK";
    
    private readonly BidirectionalCharEncoder _mappings;

    public Rotor(string characterMapping, int position = 1)
    {
        if (position < 1 || position > characterMapping.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(position), "Position must be between 1 and the length of the character mapping.");
        }
        
        _mappings = BidirectionalCharEncoder.FromMapping(characterMapping);
        Name = characterMapping[..3];
        Position = position;
    }

    public int Position { get; private set; }
    public string Name { get; init; }

    public override string ToString() => Name;

    public char Encode(char input)
    {
        input = AdjustForPositionAndRing(char.ToUpper(input), forward: true);
        char output = _mappings.Encode(input);
        output = AdjustForPositionAndRing(output, forward: false);
        
        foreach (var observer in _observers)
        {
            observer.Encoded(this, input, output);
        }
        
        return output;
    }

    public char AdjustForPositionAndRing(char input, bool forward) {
        int shift = Position - 1;
        int offset = forward ? shift : -shift;
        int adjusted = ((input - 'A') + offset + AlphabetLength) % AlphabetLength; // Ensure wrapping using modular arithmetic
        return (char)('A' + adjusted);
    }

    public bool Advance()
    {
        int initialPosition = Position;

        bool advancedNext = false;
        Position++;
        while (Position > AlphabetLength)
        {
            Position -= AlphabetLength;
            advancedNext = true;
        }
        
        foreach (var observer in _observers)
        {
            observer.Advanced(this, initialPosition, Position);
        }
        
        // TODO: This should take notch positions into account for second rotor advancement
        return advancedNext;
    }

    public void Register(IEnigmaVisitor observer) => _observers.Add(observer);
}