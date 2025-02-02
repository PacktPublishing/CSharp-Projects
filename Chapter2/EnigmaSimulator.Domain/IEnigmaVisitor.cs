namespace EnigmaSimulator.Domain;

public interface IEnigmaVisitor
{
    void Advanced(Rotor rotor, int initialPosition, int newPosition);
    void Encoded(object source, char input, char output);
    void Complete(char output);
}