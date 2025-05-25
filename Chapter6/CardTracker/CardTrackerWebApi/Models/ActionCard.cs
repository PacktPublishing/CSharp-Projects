namespace CardTrackerWebApi.Models;

public class ActionCard : Card
{
    public required string Effect { get; set; }
    public required int Cost { get; set; }
}