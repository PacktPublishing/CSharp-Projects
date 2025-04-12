namespace CardTrackerClient.Services;

public interface IUserService
{
    bool IsLoggedIn { get; }
    void Login(string token);
    void Logout();
    void AddAuthorizationHeader(HttpRequestMessage message);
}