namespace CardTrackerWebApi.Requests;

public class CreateUserRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public bool IsAdmin { get; set; }
}