namespace CardTrackerWebApi.Responses;

public abstract class CardResponse
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public string? ImagePath { get; init; }
    public abstract string Type { get; }
}