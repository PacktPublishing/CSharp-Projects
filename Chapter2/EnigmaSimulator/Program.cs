using EnigmaSimulator;
using EnigmaSimulator.Domain;
using Spectre.Console;

AnsiConsole.Write(new FigletText("Enigma").Color(Color.Gold1));

Plugboard plugboard = new("AM", "FI", "NV", "PS", "TU", "WZ");
Rotor[] rotors =
[
    new(Rotor.Enigma1, 1),
    new(Rotor.Enigma2, 1),
    new(Rotor.Enigma3, 1),
];
Reflector reflector = new(Reflector.ReflectorB);
EnigmaMachine enigmaMachine = new(rotors, plugboard, reflector);
enigmaMachine.Register(new EnigmaConsoleVisitor());

do
{
    // Read a character from the console
    ConsoleKeyInfo? keyInfo = AnsiConsole.Console.Input.ReadKey(intercept: true);
    
    char input = keyInfo.GetValueOrDefault().KeyChar;
    char output = input;
    if (char.IsLetter(input))
    {;
        output = enigmaMachine.AdvanceAndEncode(input);
    } 
    else if (!char.IsPunctuation(input))
    {
        break;
    }
    
    Console.Write(output);
} while (true);