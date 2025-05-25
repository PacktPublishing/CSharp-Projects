namespace CardTrackerClient.Services;

public interface IUserService
{
    bool IsLoggedIn { get; }
    void Logout();
    void AddAuthorizationHeader(HttpRequestMessage message);
    Task LoginAsync(string username, string password);
}