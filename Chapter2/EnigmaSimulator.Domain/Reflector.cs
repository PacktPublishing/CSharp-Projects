using EnigmaSimulator.Domain.Utilities;

namespace EnigmaSimulator.Domain;

public class Reflector(string inputMapping) : IEnigmaModule
{
    public const string ReflectorA = "EJMZALYXVBWFCRQUONTSPIKHGD";
    public const string ReflectorB = "YRUHQSLDPXNGOKMIEBFZCWVJAT";
    public const string ReflectorC = "FVPJIAOYEDRZXWGCTKUQSBNMHL";

    private readonly BidirectionalCharEncoder _mapper = new(inputMapping);

    public override string ToString() => "Reflector";

    public virtual char Encode(char input, bool isForward)
    {
        char output = _mapper.Encode(input, isForward: true);

        // Shift isForward to false to indicate we're now going backwards
        return NextModule?.Encode(output, isForward: false) ?? output;
    }

    public char Process(char input)
    {
        // Note: this will NEVER look at NextModule as the Reflector's job is to be an end-cap.
        return Encode(input, isForward: true);
    }

    public IEnigmaModule? NextModule
    {
        get => null;
        set => throw new InvalidOperationException("Reflector cannot have a next module");
    }
}