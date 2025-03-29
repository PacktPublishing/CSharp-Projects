namespace CardTrackerWebApi.Responses;

public class ActionCardResponse : CardResponse
{
    public override string Type => "Action";
    public required string Effect { get; init; }
    public int Cost { get; init; }
}