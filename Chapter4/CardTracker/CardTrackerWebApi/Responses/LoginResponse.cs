namespace CardTrackerWebApi.Responses;

public record LoginResponse
{
    public required string Token { get; set; }
}