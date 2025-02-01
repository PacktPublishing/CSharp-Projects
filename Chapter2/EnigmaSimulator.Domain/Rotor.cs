namespace EnigmaSimulator.Domain;

public class Rotor
{
    public char Encode(char input)
    {
        // Rot3 the input
        char output = (char)(input + 3);
        // If the output is greater than 'Z', wrap around to 'A'
        if (output > 'Z')
        {
            output = (char)(output - 26);
        }
        
        return output;
    }
}