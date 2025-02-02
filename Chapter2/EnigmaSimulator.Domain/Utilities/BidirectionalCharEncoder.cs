namespace EnigmaSimulator.Domain.Utilities;

public class BidirectionalCharEncoder
{
    private readonly Dictionary<char, char> _mappings;

    private BidirectionalCharEncoder()
    {
        _mappings = new Dictionary<char, char>(new CaseInsensitiveCharComparer());
    }
    
    public static BidirectionalCharEncoder FromPairs(params string[] pairs)
    {
        BidirectionalCharEncoder encoder = new();
        foreach (var pair in pairs)
        {
            if (pair.Length != 2)
            {
                throw new ArgumentException("Pairs must be exactly 2 characters long", nameof(pairs));
            }
            
            // Using Add here will throw if the key already exists. This is desirable for detecting bugs early
            encoder._mappings.Add(pair[0], pair[1]);
            encoder._mappings.Add(pair[1], pair[0]);
        }

        return encoder;
    }
    
    public static BidirectionalCharEncoder FromMapping(string mapping)
    {
        BidirectionalCharEncoder encoder = new();
        for (int i = 0; i < mapping.Length; i++)
        {
            char input = (char)('A' + i);
            char output = char.ToUpper(mapping[i]);
            encoder._mappings.Add(input, output);
        }

        if (encoder._mappings.Keys.Count != mapping.Length || 
            encoder._mappings.Values.Distinct().Count() != mapping.Length)
        {
            throw new ArgumentException("All characters must be both inputs and outputs", nameof(mapping));
        }
        
        return encoder;
    }

    public char Encode(char input) 
        => _mappings.GetValueOrDefault(input, input);
}