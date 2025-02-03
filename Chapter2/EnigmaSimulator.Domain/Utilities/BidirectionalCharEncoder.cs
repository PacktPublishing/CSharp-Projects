namespace EnigmaSimulator.Domain.Utilities;

public class BidirectionalCharEncoder
{
    private readonly Dictionary<char, char> _mappings;
    private readonly Dictionary<char, char> _reverseMappings;

    private BidirectionalCharEncoder()
    {
        _mappings = new Dictionary<char, char>(new CaseInsensitiveCharComparer());
        _reverseMappings = new Dictionary<char, char>(new CaseInsensitiveCharComparer());
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
            encoder._reverseMappings.Add(output, input);
        }

        if (encoder._mappings.Keys.Count != mapping.Length || 
            encoder._mappings.Values.Distinct().Count() != mapping.Length)
        {
            throw new ArgumentException("All characters must be both inputs and outputs", nameof(mapping));
        }
        
        return encoder;
    }

    public char Encode(char input, bool isForward, int offset = 0)
    {
        const int numLetters = 26; // Length of the alphabet
        input = char.ToUpper(input);
        
        // Adjust the input character for the offset
        int adjustedInput = ((input - 'A') + offset + numLetters) % numLetters + 'A';
        Dictionary<char, char> mappings = isForward ? _mappings : _reverseMappings;
        char encodedChar = mappings.GetValueOrDefault((char)adjustedInput, input);

        // Adjust the encoded character back for the offset
        int adjustedOutput = ((encodedChar - 'A') - offset + numLetters) % numLetters + 'A';
        return (char)adjustedOutput;
    }
}