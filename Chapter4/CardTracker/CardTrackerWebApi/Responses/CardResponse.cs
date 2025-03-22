namespace CardTrackerWebApi.Responses;

public abstract class CardResponse
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public string? ImagePath { get; init; }
    public abstract string Type { get; }
}

public class CreatureCardResponse : CardResponse
{
    public override string Type => "Creature";
    public string? SummonEffect { get; init; }
    public string? PerTurnEffect { get; init; }
    public int Cost { get; init; }
    public int Power { get; init; }
    public bool CanFly { get; init; }
    public bool CanSwim { get; init; }
    public bool CanClimb { get; init; }
}

public class ActionCardResponse : CardResponse
{
    public override string Type => "Action";
    public required string Effect { get; init; }
    public int Cost { get; init; }
}