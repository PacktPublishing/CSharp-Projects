namespace EnigmaSimulator.Domain;

public class Reflector(IEnumerable<char> inputMapping)
{
    public const string ReflectorA = "EJMZALYXVBWFCRQUONTSPIKHGD";
    public const string ReflectorB = "YRUHQSLDPXNGOKMIEBFZCWVJAT";
    public const string ReflectorC = "FVPJIAOYEDRZXWGCTKUQSBNMHL";
    
    public char Encode(char input)
    {
        // Get the index of the character in the alphabet (case-insensitive)
        int index = Rotor.Alphabet.IndexOf(char.ToUpper(input));
        
        // We can only map letters, so return input for other characters
        if (index < 0)
        {
            return input;
        }
        
        // Get the character at the same index in the character mapping
        return inputMapping.ElementAt(index);
    }
}