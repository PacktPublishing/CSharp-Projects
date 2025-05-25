namespace CardTrackerWebApi.Requests;

public abstract class CreateCardRequest
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
}