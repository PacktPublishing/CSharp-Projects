namespace EnigmaSimulator.Domain.Utilities;

public class BidirectionalCharEncoder
{
    private readonly Dictionary<char, char> _mappings;
    private readonly Dictionary<char, char> _reverseMappings;

    public BidirectionalCharEncoder(string mapping)
    {
        _mappings = new Dictionary<char, char>(new CaseInsensitiveCharComparer());
        _reverseMappings = new Dictionary<char, char>(new CaseInsensitiveCharComparer());

        for (int i = 0; i < mapping.Length; i++)
        {
            char input = (char)('A' + i);
            char output = char.ToUpper(mapping[i]);
            _mappings.Add(input, output);
            _reverseMappings.Add(output, input);
        }

        if (_mappings.Keys.Count != mapping.Length || 
            _mappings.Values.Distinct().Count() != mapping.Length)
        {
            throw new ArgumentException("All characters must be both inputs and outputs", nameof(mapping));
        }
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