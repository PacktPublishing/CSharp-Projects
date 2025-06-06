namespace CardTracker.Contracts.Requests;

public class CreateCreatureCardRequest : CreateCardRequest
{
    public string? SummonEffect { get; set; }
    public string? PerTurnEffect { get; set; }
    public int SummonCost { get; set; }
    public int Power { get; set; }
    public bool CanFly { get; set; }
    public bool CanSwim { get; set; }
    public bool CanClimb { get; set; }
}