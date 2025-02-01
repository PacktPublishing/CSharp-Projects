using EnigmaSimulator.Domain;

Rotor[] rotors =
[
    new(Rotor.Enigma1),
    new(Rotor.Enigma2),
    new(Rotor.Enigma3)
];

Pegboard pegboard = new();
Reflector reflector = new();

EnigmaMachine enigmaMachine = new(rotors, pegboard, reflector);

char input;
do
{
    // Read a character from the console
    input = Console.ReadKey().KeyChar;
    
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