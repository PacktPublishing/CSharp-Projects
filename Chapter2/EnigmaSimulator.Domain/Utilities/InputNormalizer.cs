namespace EnigmaSimulator.Domain.Utilities;

public class InputNormalizer : IEnigmaModule
{
    public IEnigmaModule? NextModule { get; set; }

    public char Encode(char input, bool isForward)
        => char.ToUpper(input);
}