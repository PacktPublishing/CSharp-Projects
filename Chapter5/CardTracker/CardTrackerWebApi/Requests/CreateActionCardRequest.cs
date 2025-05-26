namespace CardTrackerWebApi.Requests;

public class CreateActionCardRequest : CreateCardRequest
{
    public required string Effect { get; set; }
    public int Cost { get; set; }
}