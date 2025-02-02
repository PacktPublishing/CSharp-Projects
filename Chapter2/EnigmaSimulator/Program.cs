using EnigmaSimulator;
using EnigmaSimulator.Domain;
using Spectre.Console;

AnsiConsole.Write(new FigletText("Enigma").Color(Color.Gold1));

Plugboard plugboard = new();//"AM", "FI", "NV", "PS", "TU", "WZ");
Rotor[] rotors =
[
    //new(Rotor.Enigma1, 1) { Name = "Rotor 1 (I)" },
    //new(Rotor.Enigma2, 1) { Name = "Rotor 2 (II)" },
    new(Rotor.Enigma3, 1) { Name = "Rotor 1 (III)" },
    new(Rotor.Enigma3, 1) { Name = "Rotor 2 (III)" },
    new(Rotor.Enigma3, 1) { Name = "Rotor 3 (III)" },
    //new(Rotor.Enigma1, 1) { Name = "I" },
    //new(Rotor.Enigma1, 1) { Name = "I" },
    //new(Rotor.Enigma2, 1) { Name = "II" },
    //new(Rotor.Enigma3, 1) { Name = "III" }
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
        output = enigmaMachine.EncodeAndAdvance(input);
    } 
    else if (!char.IsPunctuation(input))
    {
        break;
    }
    
    Console.Write(output);
} while (true);