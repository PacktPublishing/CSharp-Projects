﻿using EnigmaSimulator;
using EnigmaSimulator.Domain;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;

try
{
    // Write the app header
    AnsiConsole.Write(new FigletText("Enigma").Color(Color.Yellow));
    AnsiConsole.WriteLine();
    AnsiConsole.Write(new Rule("[yellow]Enigma Simulator[/] by [orange3]Matt Eland[/]").LeftJustified());
    AnsiConsole.MarkupLine("[italic]This project is from Chapter 2 of [cyan]C# Projects[/] by [orange3]Packt Publishing[/][/]");
    AnsiConsole.WriteLine();

    // Define dependency injection
    ServiceCollection services = new();
    services.AddScoped<EnigmaMachine>(_ =>
    {
        Plugboard plugboard = new("AM", "FI", "NV", "PS", "TU", "WZ");
        Rotor[] rotors =
        [
            new(RotorSets.Enigma1, 1),
            new(RotorSets.Enigma2, 1),
            new(RotorSets.Enigma3, 1),
        ];
        Reflector reflector = new(Reflector.ReflectorB);
        return new EnigmaMachine(plugboard, rotors, reflector);
    });

    // Define the command line parameters
    CommandApp app = new(new TypeRegistrar(services));
    app.Configure(config =>
    {
        config.AddCommand<InteractiveEnigmaCommand>("interactive")
            .WithAlias("i")
            .WithDescription("Runs an interactive Enigma session encrypting keys as you type them.");

        config.AddCommand<EncodeCommand>("encode")
            .WithAlias("e")
            .WithDescription("Encodes a message using the Enigma machine and displays the output.")
            .WithExample("encode Hello");
    });
    app.SetDefaultCommand<InteractiveEnigmaCommand>();

    // Run the application
    return app.Run(args);
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
    return 1;
}
