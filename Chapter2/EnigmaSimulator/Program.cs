﻿using EnigmaSimulator;
using EnigmaSimulator.Domain;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;

try
{
    // Write the app header
    AnsiConsole.Write(new FigletText("Enigma").Color(Color.Black));
    AnsiConsole.WriteLine();
    AnsiConsole.Write(new Rule("[yellow]Enigma Simulator[/] by [orange3]Matt Eland[/]").LeftJustified());
    AnsiConsole.MarkupLine("[italic]This project is from Chapter 2 of [cyan]C# Projects[/] by [orange3]Packt Publishing[/][/]");
    AnsiConsole.WriteLine();

    // Define dependency injection
    ServiceCollection services = new();
    services.AddScoped<EnigmaMachine>(_ => new EnigmaMachine(
        new Plugboard(),             
        new Rotor(RotorSets.Enigma3), 
        new Rotor(RotorSets.Enigma2),
        new Rotor(RotorSets.Enigma1),
        new Reflector(ReflectorSets.ReflectorB)));

    // Define the command line parameters
    CommandApp app = new(new TypeRegistrar(services));
    app.Configure(config =>
    {
        config.AddCommand<InteractiveEnigmaCommand>("interactive")
            .WithAlias("i")
            .WithDescription("Encrypts keystrokes as you type them using Enigma");

        config.AddCommand<EncodeCommand>("encode")
            .WithAlias("e")
            .WithDescription("Encodes a message using and displays the output.")
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
