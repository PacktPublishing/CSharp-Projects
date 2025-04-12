using System.Net.Http.Headers;

namespace CardTrackerClient.Services;

public class UserService : IUserService
{
    private string? _token;
    
    public bool IsLoggedIn => !string.IsNullOrEmpty(_token);
    
    public void Login(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            throw new ArgumentException("Token cannot be null or empty.", nameof(token));
        }
        _token = token;
    }
    
    public void Logout() => _token = null;

    public void AddAuthorizationHeader(HttpRequestMessage message)
    {
        if (string.IsNullOrEmpty(_token))
        {
            throw new InvalidOperationException("User is not logged in.");
        }
        
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
    }
}