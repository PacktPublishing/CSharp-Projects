using EnigmaSimulator.Domain;

Rotor[] rotors = new Rotor[]
{
    new Rotor(),
    new Rotor(),
    new Rotor()
};

Pegboard pegboard = new Pegboard();
Reflector reflector = new Reflector();

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