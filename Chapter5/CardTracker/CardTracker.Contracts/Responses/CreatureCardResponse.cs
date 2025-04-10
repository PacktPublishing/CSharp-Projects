namespace CardTracker.Contracts.Responses;

public class CreatureCardResponse : CardResponse
{
    public override string Type => "Creature";
    public string? SummonEffect { get; init; }
    public string? PerTurnEffect { get; init; }
    public int SummonCost { get; init; }
    public int Power { get; init; }
    public bool CanFly { get; init; }
    public bool CanSwim { get; init; }
    public bool CanClimb { get; init; }
}