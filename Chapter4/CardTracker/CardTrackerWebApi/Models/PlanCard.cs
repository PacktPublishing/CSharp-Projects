namespace CardTrackerWebApi.Models;

public record PlanCard : Card
{
    public string Effect { get; set; }
    public required string Requirements { get; set; }
    public string? ResolvedEffect { get; set; }
    public required int CardsDrawnPerTurn { get; set; }
    public required int CunningPerTurn { get; set; }
    public required int VictoryPointsOnCompleted { get; set; }
}