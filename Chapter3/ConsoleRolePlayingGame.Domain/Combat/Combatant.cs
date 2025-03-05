namespace ConsoleRolePlayingGame.Domain.Combat;

public record Combatant
{
    public Combatant(CombatantData data)
    {
        Name = data.Name;
        AsciiArt = data.AsciiArt;
        MaxHealth = data.MaxHealth;
        Health = MaxHealth;
        MaxMana = data.MaxMana;
        Mana = MaxMana;
        Strength = data.Strength;
        Dexterity = data.Dexterity;
        Intelligence = data.Intelligence;
        Speed = data.Speed;
    }

    public string Name { get; init; }
    public string[] AsciiArt { get; init; }
    
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Mana { get; set; }
    public int MaxMana { get; set; }
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Intelligence { get; set; }
    public int Speed { get; set; }
    
    public bool IsDead => Health <= 0;
    public IEnumerable<Ability> Abilities { get; set; } = [];

    public int TimeUntilTurn { get; internal set; }
    
    public bool IsReady => TimeUntilTurn <= 0 && !IsDead;
    public bool IsPlayer { get; set; }
    public required IBattleStrategy Strategy { get; init; }
}