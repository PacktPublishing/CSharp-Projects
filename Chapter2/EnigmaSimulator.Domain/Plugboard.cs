using EnigmaSimulator.Domain.Utilities;

namespace EnigmaSimulator.Domain;

public class Plugboard : IEnigmaModule
{
    private readonly Dictionary<char, char> _mappings = new();

    public Plugboard(params string[] pairs)
    {
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

    public char Encode(char input, bool isForward = true) 
        => _mappings.GetValueOrDefault(input, defaultValue: input);

    public IEnigmaModule? NextModule { get; set; }
}