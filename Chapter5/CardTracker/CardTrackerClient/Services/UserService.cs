using System.Net.Http.Headers;
using System.Net.Http.Json;
using CardTracker.Contracts.Requests;
using CardTracker.Contracts.Responses;

namespace CardTrackerClient.Services;

public class UserService(HttpClient client) : IUserService
{
    private string? _token;
    
    public bool IsLoggedIn => !string.IsNullOrEmpty(_token);
    
    public async Task LoginAsync(string username, string password)
    {
        LoginRequest loginRequest = new()
        {
            Username = username,
            Password = password
        };

        HttpResponseMessage response = await client.PostAsJsonAsync("/login", loginRequest);
        response.EnsureSuccessStatusCode();

        LoginResponse loginResponse = (await response.Content.ReadFromJsonAsync<LoginResponse>())!;
        _token = loginResponse.Token;
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