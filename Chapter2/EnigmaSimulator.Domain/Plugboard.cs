using EnigmaSimulator.Domain.Utilities;

namespace EnigmaSimulator.Domain;

public class Plugboard : IEnigmaModule
{
    private readonly Dictionary<char, char> _mappings;

    public Plugboard(params string[] pairs)
    {
        _mappings = new Dictionary<char, char>(new CaseInsensitiveCharComparer());
        
        foreach (string pair in pairs)
        {
            if (pair.Length != 2)
            {
                throw new ArgumentException("Each pair must consist of exactly two characters.");
            }

            _mappings.Add(char.ToUpper(pair[0]), char.ToUpper(pair[1]));
            _mappings.Add(char.ToUpper(pair[1]), char.ToUpper(pair[0]));
        }
    }

    public override string ToString() => "Plugboard";

    public char Encode(char input, bool isForward) 
        => _mappings.GetValueOrDefault(input, input);

    public IEnigmaModule? NextModule { get; set; }
}