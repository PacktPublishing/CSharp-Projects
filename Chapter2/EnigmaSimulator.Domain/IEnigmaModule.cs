namespace EnigmaSimulator.Domain;

public interface IEnigmaModule
{
    IEnigmaModule? NextModule { get; set; }
    char Encode(char input, bool isForward);
    char Process(char input)
    {
        char output = Encode(input, isForward: true);

        output = NextModule?.Process(output) ?? output;

        return Encode(output, isForward: false);
    }

}