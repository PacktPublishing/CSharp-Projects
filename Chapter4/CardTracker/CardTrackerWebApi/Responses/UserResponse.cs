namespace CardTrackerWebApi.Responses;

public class UserResponse
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public bool IsAdmin { get; set; }
}