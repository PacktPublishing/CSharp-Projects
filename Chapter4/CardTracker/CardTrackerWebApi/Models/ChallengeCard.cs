namespace CardTrackerWebApi.Models;

public record ChallengeCard : Card
{
    public string? RevealedEffect { get; set; }
    public string? ResolvedEffect { get; set; }
    public string? PerTurnEffect { get; set; }
    public required int KnowledgeRequired { get; set; }
    public required int FoodRequired { get; set; }
    public required int VictoryPointsOnCompleted { get; set; }
}