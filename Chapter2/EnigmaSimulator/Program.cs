using EnigmaSimulator.Domain;

Pegboard pegboard = new("AM", "FI", "NV", "PS", "TU", "WZ");
Rotor[] rotors =
[
    new(Rotor.Enigma2, 24),
    new(Rotor.Enigma1, 13),
    new(Rotor.Enigma3, 22)
];
Reflector reflector = new(Reflector.ReflectorA);
EnigmaMachine enigmaMachine = new(rotors, pegboard, reflector);

do
{
    // Read a character from the console
    char input = Console.ReadKey().KeyChar;
    
    char output = input;
    if (char.IsLetter(input))
    {;
        output = enigmaMachine.Encode(input);
    } 
    else if (!char.IsPunctuation(input))
    {
        break;
    }
    
    Console.Write(output);
} while (true);