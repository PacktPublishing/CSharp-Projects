using EnigmaSimulator.Domain.Utilities;

namespace EnigmaSimulator.Domain;

public class EnigmaMachine
{
    public EnigmaMachine(params IEnigmaModule[] modules)
    {
        // Auto-add standard filters and advancers if not present
        if (!modules.OfType<RotorAdvancer>().Any())
        {
            modules = [
                new InputFilter(),
                new InputNormalizer(),
                new RotorAdvancer(),
                ..modules
            ];
        }
        
        NextModule = modules[0];
        for (int i = 0; i < modules.Length - 1; i++)
        {
            modules[i].NextModule = modules[i + 1];
        }
    }

    public IEnigmaModule? NextModule { get; set; }

    public char Encode(char input) => NextModule?.Process(input) ?? input;
    public string Encode(string input)
    {
        if (string.IsNullOrWhiteSpace(input) || NextModule is null)
        {
            return input;
        }

        char[] encodedLetters = input.Select(NextModule.Process).ToArray();
        return new string(encodedLetters);
    }
}