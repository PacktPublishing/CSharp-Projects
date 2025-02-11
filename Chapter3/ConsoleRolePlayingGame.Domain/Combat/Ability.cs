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
}