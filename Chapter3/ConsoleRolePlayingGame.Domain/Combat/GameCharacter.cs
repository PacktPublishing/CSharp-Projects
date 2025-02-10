namespace ConsoleRolePlayingGame.Domain.Combat;

public record class GameCharacter
{
    public required string Name { get; init; }
    public required string[] AsciiArt { get; init; }
    
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Mana { get; set; }
    public int MaxMana { get; set; }
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Intelligence { get; set; }
    public int Speed { get; set; }
    
    public bool IsDead => Health <= 0;
}