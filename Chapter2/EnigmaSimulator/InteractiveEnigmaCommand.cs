using Spectre.Console;
using EnigmaSimulator.Domain;
using Spectre.Console.Cli;

namespace EnigmaSimulator;

public class InteractiveEnigmaCommand(EnigmaMachine enigma) : Command
{
    public override int Execute(CommandContext context)
    {
        AnsiConsole.MarkupLine("Enigma will encode letters you type. Press [yellow bold]Enter[/] to exit.");
        AnsiConsole.WriteLine();

        char output;
        do
        {
            ConsoleKeyInfo? keyInfo = AnsiConsole.Console.Input.ReadKey(intercept: true);

            char input = keyInfo.GetValueOrDefault().KeyChar;
            output = enigma.Process(input);

            if (!char.IsWhiteSpace(output))
            {
                AnsiConsole.Write(output);
            }

        } while (!Environment.NewLine.Contains(output));

        return 0;
    }
}