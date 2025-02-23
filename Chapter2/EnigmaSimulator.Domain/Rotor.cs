using EnigmaSimulator.Domain.Utilities;

namespace EnigmaSimulator.Domain;

public class Rotor : IEnigmaModule
{
    private readonly HashSet<char> _notches;
    private readonly BidirectionalCharEncoder _mappings;

    public Rotor(string characterMapping, int position = 1)
    {
        Position = position;
        
        // Split the character mapping into the main mapping and notches array
        string[] parts = characterMapping.Split('-');
        _mappings = new BidirectionalCharEncoder(parts[0]);

        _notches = parts.Length > 1 ? [..parts[1]] : [];
    }

    public int Position { get; private set; }

    public IEnigmaModule? NextModule { get; set; }

    public char Encode(char input, bool isForward) 
        => _mappings.Encode(input, isForward, offset: Position - 1);
    
    public bool HasNotch(int position)
        => _notches.Contains((char)('A' + position - 1));

    public bool Advance()
    {
        bool hadNotch = HasNotch(Position);

        // Increment the position and ensure it wraps around
        const int numLetters = 26;
        Position = (Position % numLetters) + 1;
        
        return hadNotch;
    }
}