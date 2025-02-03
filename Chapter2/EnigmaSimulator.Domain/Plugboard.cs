using EnigmaSimulator.Domain.Utilities;

namespace EnigmaSimulator.Domain;

public class Plugboard
{
    private readonly Dictionary<char, char> _mappings;
    private readonly List<IEnigmaVisitor> _observers = [];

    public Plugboard(params string[] pairs)
    {
        _mappings = new Dictionary<char, char>(new CaseInsensitiveCharComparer());
        
        foreach (string pair in pairs)
        {
            if (pair.Length != 2)
            {
                throw new ArgumentException("Each pair must consist of exactly two characters.");
            }

            char first = char.ToUpper(pair[0]);
            char second = char.ToUpper(pair[1]);

            _mappings.Add(first, second);
            _mappings.Add(second, first);
        }
    }

    public override string ToString() => "Plugboard";

    public char Encode(char input)
    {
        var output = _mappings.GetValueOrDefault(input, input);
        
        foreach (var observer in _observers)
        {
            observer.Encoded(this, input, output);
        }
        
        return output;
    }

    public void Register(IEnigmaVisitor observer) => _observers.Add(observer);
}