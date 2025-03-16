namespace CardTrackerWebApi.Models;

public record LocationCard : Card
{
    public string? RevealedEffect { get; set; }
    public string? UnexploredPerTurnEffect { get; set; }
    public string? ExploredEffect { get; set; }
    public string? ExploredPerTurnEffect { get; set; }
    public required int Challenges { get; set; }
    public required int VictoryPointsOnExplored { get; set; }
}