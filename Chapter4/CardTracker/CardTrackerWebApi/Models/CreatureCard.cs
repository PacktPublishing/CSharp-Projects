namespace CardTrackerWebApi.Models;

public class CreatureCard : Card
{
    public string? SummonEffect { get; set; }
    public string? PerTurnEffect { get; set; }
    public required int SummonCost { get; set; }
    public required int Power { get; set; }
    public bool CanFly { get; set; }
    public bool CanSwim { get; set; }
    public bool CanClimb { get; set; }
}