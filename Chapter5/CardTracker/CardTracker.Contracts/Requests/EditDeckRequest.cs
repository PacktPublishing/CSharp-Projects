namespace CardTracker.Contracts.Requests;

public class EditDeckRequest
{
    public required string Name { get; set; }

    public required List<int> CardIds { get; set; } = [];
}