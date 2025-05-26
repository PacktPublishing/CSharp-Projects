namespace CardTracker.Contracts.Responses;

public class UserResponse
{
    public int Id { get; init; }
    public required string Username { get; init; }
    public bool IsAdmin { get; init; }
}