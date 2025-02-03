using EnigmaSimulator.Domain;
using Spectre.Console;

namespace EnigmaSimulator;

public class EnigmaConsoleVisitor : IEnigmaVisitor
{
    public void Advanced(Rotor rotor, int initialPosition, int newPosition)
    {
        AnsiConsole.WriteLine("{0} advanced from {1} to {2}", rotor, initialPosition, newPosition);
    }

    public void Encoded(object source, char input, char output)
    {
        AnsiConsole.WriteLine("{0} encoded {1} to {2}", source, input, output);
    }

    public void Complete(char output)
    {
        AnsiConsole.WriteLine("Output: {0}", output);
    }
}