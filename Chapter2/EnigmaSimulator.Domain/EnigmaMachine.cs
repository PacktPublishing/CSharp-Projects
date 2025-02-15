using EnigmaSimulator.Domain.Utilities;

namespace EnigmaSimulator.Domain;

public class EnigmaMachine : IEnigmaModule
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

    public string Encode(string input) 
        => new string(input.Select(Process).ToArray());

    public char Process(char input) 
        => NextModule?.Process(input) ?? input;
}