namespace EnigmaSimulator.Domain;

public class Rotor(string characterMapping, int position = 1)
{
    private readonly List<IEnigmaVisitor> _observers = [];
    
    public const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    
    public const string Enigma1 = "EKMFLGDQVZNTOWYHXUSPAIBRCJ";
    public const string Enigma2 = "AJDKSIRUXBLHWTMCQGZNPYFVOE";
    public const string Enigma3 = "BDFHJLCPRTXVZNYEIWGAKMUSQO";
    public const string Enigma4 = "ESOVPZJAYQUIRHXLNFTGKDCMWB";
    public const string Enigma5 = "VZBRGITYUPSDNHLXAWMJQOFECK";

    public int Position { get; private set; } = position;
    public string Name { get; set; } = characterMapping[..3];

    public override string ToString() => Name;

    public char Encode(char input)
    {
        char output = input;
     
        // Get the index of the character in the alphabet (case-insensitive)
        int index = Alphabet.IndexOf(input);
        
        // We can only map letters, so return input for other characters
        if (index >= 0)
        {
            output = characterMapping.ElementAt(index);
            
            // Add the position to the output
            output = (char)(output + Position - 1);
            // Offset the output by the rotor's position
            //index = (index - Position + characterMapping.Length) % characterMapping.Length;
            //output = Alphabet.ElementAt(index);
        }
        
        foreach (var observer in _observers)
        {
            observer.Encoded(this, input, output);
        }
        
        return output;
    }

    public char EncodeReverse(char input)
    {
        char output = input;
        
        // Get the index of the character in the alphabet (case-insensitive)
        int index = characterMapping.IndexOf(input);

        if (index >= 0)
        {
            // Adjust index by position
            index = (index - Position + 1 + characterMapping.Length) % characterMapping.Length;
            
            output = Alphabet.ElementAt(index);
            
            // Add the position to the output
            //output = (char)(output - Position + 1);
        }
        
        foreach (var observer in _observers)
        {
            observer.Encoded(this, input, output);
        }
        
        return output;
    }

    public bool Advance()
    {
        int initialPosition = Position;

        bool advancedNext = false;
        Position++;
        while (Position > characterMapping.Length)
        {
            Position -= characterMapping.Length;
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