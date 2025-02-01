namespace EnigmaSimulator.Domain;

public class Rotor(IEnumerable<char> characterMapping)
{
    public const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public const string Enigma1 = "EKMFLGDQVZNTOWYHXUSPAIBRCJ";
    public const string Enigma2 = "AJDKSIRUXBLHWTMCQGZNPYFVOE";
    public const string Enigma3 = "BDFHJLCPRTXVZNYEIWGAKMUSQO";
    public const string Enigma4 = "ESOVPZJAYQUIRHXLNFTGKDCMWB";
    public const string Enigma5 = "VZBRGITYUPSDNHLXAWMJQOFECK";
    public const string ReflectorA = "EJMZALYXVBWFCRQUONTSPIKHGD";
    public const string ReflectorB = "YRUHQSLDPXNGOKMIEBFZCWVJAT";
    public const string ReflectorC = "FVPJIAOYEDRZXWGCTKUQSBNMHL";
 
    public char Encode(char input)
    {
        // Get the index of the character in the alphabet (case-insensitive)
        int index = Alphabet.IndexOf(char.ToUpper(input));
        
        // We can only map letters, so return input for other characters
        if (index < 0)
        {
            return input;
        }
        
        // Get the character at the same index in the character mapping
        return characterMapping.ElementAt(index);
    }

}