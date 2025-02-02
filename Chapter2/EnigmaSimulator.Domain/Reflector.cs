using EnigmaSimulator.Domain.Utilities;

namespace EnigmaSimulator.Domain;

public class Reflector(string inputMapping)
{
    private readonly List<IEnigmaVisitor> _observers = [];
    public const string ReflectorA = "EJMZALYXVBWFCRQUONTSPIKHGD";
    public const string ReflectorB = "YRUHQSLDPXNGOKMIEBFZCWVJAT";
    public const string ReflectorC = "FVPJIAOYEDRZXWGCTKUQSBNMHL";

    private readonly BidirectionalCharEncoder _mapper = BidirectionalCharEncoder.FromMapping(inputMapping);
    
    public override string ToString() => "Reflector";

    public virtual char Encode(char input)
    {
        char output = _mapper.Encode(input);

        foreach (var observer in _observers)
        {
            observer.Encoded(this, input, output);
        }
        
        return output;
    }

    public void Register(IEnigmaVisitor observer) => _observers.Add(observer);
}