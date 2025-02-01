namespace EnigmaSimulator.Domain;

internal class CaseInsensitiveCharComparer : IEqualityComparer<char>
{
    public bool Equals(char x, char y) 
        => char.ToUpper(x) == char.ToUpper(y);

    public int GetHashCode(char input) 
        => char.ToUpper(input).GetHashCode();
}