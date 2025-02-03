namespace EnigmaSimulator.Domain.Utilities;

public class InputFilter : IEnigmaModule
{
    public IEnigmaModule? NextModule { get; set; }

    public char Process(char input)
    {
        if (!char.IsLetter(input))
        {
            return input;
        }

        return NextModule?.Process(input) ?? input;
    }
}