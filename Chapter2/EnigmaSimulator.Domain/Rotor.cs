using EnigmaSimulator.Domain.Utilities;

namespace EnigmaSimulator.Domain;

public class Rotor
{
    private readonly List<IEnigmaVisitor> _observers = [];
    private readonly HashSet<char> _notches = new(new CaseInsensitiveCharComparer());
    
    public const string Enigma1 = "EKMFLGDQVZNTOWYHXUSPAIBRCJ-Q";
    public const string Enigma2 = "AJDKSIRUXBLHWTMCQGZNPYFVOE-E";
    public const string Enigma3 = "BDFHJLCPRTXVZNYEIWGAKMUSQO-V";
    public const string Enigma4 = "ESOVPZJAYQUIRHXLNFTGKDCMWB-J";
    public const string Enigma5 = "VZBRGITYUPSDNHLXAWMJQOFECK-Z";
    
    private readonly BidirectionalCharEncoder _mappings;

    public Rotor(string characterMapping, int position = 1)
    {
        Position = position;
        
        // Split the character mapping into the main mapping and notches array
        string[] parts = characterMapping.Split('-');
        _mappings = new BidirectionalCharEncoder(parts[0]);

        _notches = parts.Length > 1
            ? new HashSet<char>(parts[1], new CaseInsensitiveCharComparer())
            : [];
    }

    public int Position { get; private set; }

    public char Encode(char input, bool isForward)
    {
        char output = _mappings.Encode(input, isForward, offset: Position - 1);

        foreach (var observer in _observers)
        {
            observer.Encoded(this, input, output);
        }

        return output;
    }

    public bool Advance()
    {
        const int numLetters = 26;
        int initialPosition = Position;

        Position++;
        while (Position > numLetters)
        {
            Position -= numLetters;
        }
        
        foreach (var observer in _observers)
        {
            observer.Advanced(this, initialPosition, Position);
        }
        
        // Check if the rotor is at a notch position
        return _notches.Contains((char)('A' + initialPosition - 1));
    }

    public void Register(IEnigmaVisitor observer) => _observers.Add(observer);
}