using System.ComponentModel;

namespace EnigmaSimulator.Domain;

public class Pegboard
{
    private readonly Dictionary<char, char> _mappings 
        = new(new CaseInsensitiveCharComparer());

    public Pegboard(params string[] pairs)
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
        => _mappings.ContainsKey(char.ToUpper(input)) 
            ? _mappings[char.ToUpper(input)] 
            : input;

    private void Connect(char c1, char c2)
    {
        _mappings.Add(c1, c2);
        _mappings.Add(c2, c1);
    }
}