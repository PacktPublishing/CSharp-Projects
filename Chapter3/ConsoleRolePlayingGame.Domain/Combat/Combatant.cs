namespace ConsoleRolePlayingGame.Domain.Combat;

public record Combatant
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

    public IEnumerable<string> AbilityIds { get; init; } = [];
    public IEnumerable<Ability> Abilities { get; set; } = [];

    public int TimeUntilTurn { get; internal set; }
    
    public bool IsReady => TimeUntilTurn <= 0 && !IsDead;
    public bool IsPlayer { get; set; }
    public IBattleStrategy Strategy { get; init; }
}