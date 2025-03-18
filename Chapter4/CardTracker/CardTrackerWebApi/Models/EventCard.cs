namespace CardTrackerWebApi.Models;

public record EventCard : Card
{
    public required string Effect { get; set; }
}