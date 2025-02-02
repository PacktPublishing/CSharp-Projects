using System.ComponentModel;

namespace EnigmaSimulator.Domain;

public class Plugboard
{
    private readonly Dictionary<char, char> _mappings 
        = new(new CaseInsensitiveCharComparer());

    private readonly List<IEnigmaVisitor> _observers = [];

    public override string ToString() => "Plugboard";

    public Plugboard(params string[] pairs)
    {
        foreach (var pair in pairs)
        {
            if (pair.Length != 2)
            {
                throw new ArgumentException("Pairs must be exactly 2 characters long", nameof(pairs));
            }

            Connect(pair[0], pair[1]);
        }
    }

    public char Encode(char input)
    {
        char output = input;
        
        if (_mappings.ContainsKey(char.ToUpper(input)))
        {
            output = _mappings[char.ToUpper(input)];
        }

        foreach (var observer in _observers)
        {
            observer.Encoded(this, input, output);
        }
        
        return output;
    }

    private void Connect(char c1, char c2)
    {
        _mappings.Add(c1, c2);
        _mappings.Add(c2, c1);
    }

    public void Register(IEnigmaVisitor observer)
    {
        _observers.Add(observer);
    }
}