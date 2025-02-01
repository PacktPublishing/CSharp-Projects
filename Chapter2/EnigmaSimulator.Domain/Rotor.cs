namespace EnigmaSimulator.Domain;

public class Rotor(IEnumerable<char> characterMapping, int position = 0)
{
    public const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public const string Enigma1 = "EKMFLGDQVZNTOWYHXUSPAIBRCJ";
    public const string Enigma2 = "AJDKSIRUXBLHWTMCQGZNPYFVOE";
    public const string Enigma3 = "BDFHJLCPRTXVZNYEIWGAKMUSQO";
    public const string Enigma4 = "ESOVPZJAYQUIRHXLNFTGKDCMWB";
    public const string Enigma5 = "VZBRGITYUPSDNHLXAWMJQOFECK";
    
    public int Position { get; private set; } = position % characterMapping.Count();

    public char Encode(char input)
    {
        // Get the index of the character in the alphabet (case-insensitive)
        int index = Alphabet.IndexOf(char.ToUpper(input));
        
        // Take into account the rotor's position, wrapping around if necessary
        index = (index + Position) % characterMapping.Count();
        
        // We can only map letters, so return input for other characters
        if (index < 0)
        {
            return input;
        }
        
        // Get the character at the same index in the character mapping
        return characterMapping.ElementAt(index);
    }

    public bool Advance()
    {
        Position = (Position + 1) % characterMapping.Count();
        
        // TODO: This should take notch positions into account for second rotor advancement
        return false;
    }
}