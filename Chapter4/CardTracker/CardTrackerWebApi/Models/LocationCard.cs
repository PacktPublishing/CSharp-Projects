namespace CardTrackerWebApi.Models;

public record LocationCard : Card
{
    public string? RevealedEffect { get; set; }
    public string? ExploredEffect { get; set; }
    public required int Challenges { get; set; }
    public required int VictoryPointsOnExplored { get; set; }
}