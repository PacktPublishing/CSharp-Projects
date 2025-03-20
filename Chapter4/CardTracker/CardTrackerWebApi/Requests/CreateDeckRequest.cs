namespace CardTrackerWebApi.Requests;

public class CreateDeckRequest
{
    public required string Name { get; set; }
    public required List<int> CardIds { get; set; } = [];
}