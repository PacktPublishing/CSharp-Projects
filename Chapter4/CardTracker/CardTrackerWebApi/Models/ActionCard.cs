namespace CardTrackerWebApi.Models;

public record ActionCard : Card
{
    public required string Effect { get; set; }
    public required int Cost { get; set; }
}