using EnigmaSimulator.Domain.Utilities;

namespace EnigmaSimulator.Domain;

public class Plugboard(params string[] pairs)
{
    private readonly BidirectionalCharEncoder _mappings = BidirectionalCharEncoder.FromPairs(pairs);

    private readonly List<IEnigmaVisitor> _observers = [];

    public override string ToString() => "Plugboard";

    public char Encode(char input)
    {
        char output = _mappings.Encode(input, isForward: true);

        foreach (var observer in _observers)
        {
            observer.Encoded(this, input, output);
        }
        
        return output;
    }

    public void Register(IEnigmaVisitor observer) => _observers.Add(observer);
}