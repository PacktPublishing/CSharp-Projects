namespace CardTrackerWebApi.Models;

public record FriendCard : Card
{
    public string? SummonEffect { get; set; }
    public string? PerTurnEffect { get; set; }
    public required int SummonCost { get; set; }
    public required int FoodPerTurn { get; set; }
    public required int Power { get; set; }
    public bool CanFly { get; set; }
    public bool CanSwim { get; set; }
    public bool CanDig { get; set; }
    public bool CanClimb { get; set; }
}