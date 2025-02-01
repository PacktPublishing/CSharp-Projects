using System.ComponentModel;

namespace EnigmaSimulator.Domain;

public class Pegboard
{
    private readonly Dictionary<char, char> _mappings 
        = new(new CaseInsensitiveCharComparer());
    
    public char Encode(char input) 
        => HasConnection(input) 
            ? _mappings[char.ToUpper(input)] 
            : input;

    public void Connect(char c1, char c2)
    {
        _mappings.Add(c1, c2);
        _mappings.Add(c2, c1);
    }

    public bool HasConnection(char c) 
        => _mappings.ContainsKey(char.ToUpper(c));
}