namespace ConsoleRolePlayingGame.Domain.Combat;

public record Ability
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public bool TargetsSingle { get; init; }
    public bool IsHeal { get; init; }
    public int ManaCost { get; init; }
    public float MinMultiplier { get; init; }
    public float MaxMultiplier { get; init; }
    public Trait Attribute { get; init; } = Trait.None;

    public int CalculateAmount(Combatant activator, Combatant target, Random random)
    {
        int mult = Attribute switch
        {
            Trait.Strength => activator.Strength,
            Trait.Dexterity => activator.Dexterity,
            Trait.Intelligence => activator.Intelligence,
            _ => 1
        };
        float amount = random.Next((int)(MinMultiplier * mult), (int)(MaxMultiplier * mult));

        return (int)Math.Round(amount);
    }
}