namespace EnigmaSimulator.Domain.Utilities;

public class InputFilter : IEnigmaModule
{
    public IEnigmaModule? NextModule { get; set; }

    public char Encode(char input, bool isForward) => input;
    
    public char Process(char input) 
        => NextModule is not null && char.IsLetter(input) 
            ? NextModule.Process(input) 
            : input;
}