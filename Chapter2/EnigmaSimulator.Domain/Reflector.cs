using EnigmaSimulator.Domain.Utilities;

namespace EnigmaSimulator.Domain;

public class Reflector(string inputMapping) : IEnigmaModule
{
    private readonly BidirectionalCharEncoder _mapper = new(inputMapping);
    public IEnigmaModule? NextModule
    {
        get => null;
        set => throw new InvalidOperationException("Reflector cannot have a next module.");
    }

    public virtual char Encode(char input, bool isForward = true) => _mapper.Encode(input, isForward);
    
    public char Process(char input) => Encode(input, isForward: true);
}