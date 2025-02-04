using EnigmaSimulator.Domain;
using Spectre.Console;
using Spectre.Console.Cli;
using ValidationResult = Spectre.Console.ValidationResult;

namespace EnigmaSimulator;

public class EncodeCommand(EnigmaMachine enigma) : Command<EncodeCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "[Input]")] 
        public required string Input { get; init; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        string output = enigma.Encode(settings.Input);
        AnsiConsole.MarkupLine($"Output: [yellow]{output}[/]");

        return 0;
    }

    public override ValidationResult Validate(CommandContext context, Settings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.Input))
        {
            return ValidationResult.Error("Input is required");
        }

        return base.Validate(context, settings);
    }
}