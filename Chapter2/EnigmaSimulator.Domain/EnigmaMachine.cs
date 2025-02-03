using EnigmaSimulator.Domain.Utilities;

namespace EnigmaSimulator.Domain;

public class EnigmaMachine : IEnigmaModule
{
    public EnigmaMachine(Plugboard plugboard, Rotor[] rotors, Reflector reflector)
    {
        // We'll want to filter out non-letters, ensure capitalization, and always advance
        InputFilter filter = new();
        NextModule = filter;
        InputNormalizer normalizer = new();
        filter.NextModule = normalizer;
        RotorAdvancer advancer = new(rotors);
        normalizer.NextModule = advancer;

        // Connect the plugboard
        advancer.NextModule = plugboard;
        IEnigmaModule lastModule = plugboard;

        // We enter rotors from the right but think about them left to right
        foreach (var rotor in rotors.Reverse())
        {
            lastModule.NextModule = rotor;
            lastModule = rotor;
        }

        // Our last module should always be the reflector
        lastModule.NextModule = reflector;
    }

    public IEnigmaModule? NextModule { get; set; }

    public string Encode(string input) 
        => new string(input.Select(Process).ToArray());

    public char Process(char input) 
        => NextModule?.Process(input) ?? input;
}