namespace EnigmaSimulator.Domain.Utilities;

public class BidirectionalCharEncoder
{
    private readonly Dictionary<char, char> _mappings = new(new CaseInsensitiveCharComparer());

    private BidirectionalCharEncoder()
    {
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

            encoder.Connect(pair[0], pair[1]);
        }

        return encoder;
    }
    
    public static BidirectionalCharEncoder FromMapping(string mapping)
    {
        mapping = mapping.ToUpper();
        
        BidirectionalCharEncoder encoder = new();
        for (int i = 0; i < mapping.Length; i++)
        {
            char input = (char)('A' + i);
            char output = mapping[i];
            encoder._mappings.Add(input, output);
        }

        if (encoder._mappings.Keys.Count != mapping.Length || encoder._mappings.Values.Distinct().Count() != mapping.Length)
        {
            throw new ArgumentException("All characters must be both inputs and outputs", nameof(mapping));
        }
        
        return encoder;
    }

    private void Connect(char a, char b)
    {
        // Using Add here will throw if the key already exists. This is desirable for detecting bugs early
        _mappings.Add(a, b);
        _mappings.Add(b, a);
    }
    
    public char Encode(char input)
    {
        if (!_mappings.ContainsKey(input))
        {
            return input;
        }
        
        return _mappings[char.ToUpper(input)];
    }
}