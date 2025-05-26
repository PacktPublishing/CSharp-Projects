namespace ConsoleRolePlayingGame.CombatSystem;

public record CombatantData
{
    public required string Name { get; init; }
    public required string[] AsciiArt { get; init; }
    
    public int MaxHealth { get; set; }
    public int MaxMana { get; set; }
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Intelligence { get; set; }
    public int Speed { get; set; }

    public IEnumerable<string> AbilityIds { get; init; } = [];
}