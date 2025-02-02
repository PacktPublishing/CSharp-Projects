namespace EnigmaSimulator.Domain;

public class Reflector(string inputMapping)
{
    private readonly List<IEnigmaVisitor> _observers = [];
    public const string ReflectorA = "EJMZALYXVBWFCRQUONTSPIKHGD";
    public const string ReflectorB = "YRUHQSLDPXNGOKMIEBFZCWVJAT";
    public const string ReflectorC = "FVPJIAOYEDRZXWGCTKUQSBNMHL";

    public override string ToString() => "Reflector";

    public char Encode(char input)
    {
        // Get the index of the character in the alphabet (case-insensitive)
        int index = Rotor.Alphabet.IndexOf(char.ToUpper(input));
        
        // We can only map letters, so return input for other characters
        char output = input;
        if (index >= 0)
        {
            output = inputMapping.ElementAt(index);
        }

        foreach (var observer in _observers)
        {
            observer.Encoded(this, input, output);
        }
        
        return output;
    }

    public void Register(IEnigmaVisitor observer) => _observers.Add(observer);
}