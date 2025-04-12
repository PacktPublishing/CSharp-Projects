using System.Net.Http.Json;
using CardTracker.Contracts.Requests;
using CardTracker.Contracts.Responses;

namespace CardTrackerClient.Services;

public class CardApiService(HttpClient client, IUserService userService) : ICardApiService
{
    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("/login", loginRequest);
        response.EnsureSuccessStatusCode();

        LoginResponse loginResponse = (await response.Content.ReadFromJsonAsync<LoginResponse>())!;
        userService.Login(loginResponse.Token);
        
        return loginResponse;
    }
    
    public async Task<List<DeckResponse>> GetAllDecksAsync()
    {
        HttpRequestMessage httpRequest = new(HttpMethod.Get, "/decks/");
        userService.AddAuthorizationHeader(httpRequest);

        HttpResponseMessage response = await client.SendAsync(httpRequest);
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<List<DeckResponse>>())!;
    }

    public async Task<DeckResponse> GetDeckByIdAsync(int id)
    {
        HttpRequestMessage httpRequest = new(HttpMethod.Get, $"/decks/{id}");
        userService.AddAuthorizationHeader(httpRequest);
        
        HttpResponseMessage response = await client.SendAsync(httpRequest);
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<DeckResponse>())!;
    }

    public async Task<DeckResponse> CreateDeckAsync(CreateDeckRequest request)
    {
        HttpRequestMessage httpRequest = new(HttpMethod.Post, "/decks")
        {
            Content = JsonContent.Create(request)
        };
        userService.AddAuthorizationHeader(httpRequest);

        HttpResponseMessage response = await client.SendAsync(httpRequest);
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<DeckResponse>())!;
    }

    public async Task<DeckResponse> UpdateDeckAsync(int id, EditDeckRequest request)
    {
        HttpRequestMessage httpRequest = new(HttpMethod.Put, $"/decks/{id}")
        {
            Content = JsonContent.Create(request)
        };
        userService.AddAuthorizationHeader(httpRequest);

        HttpResponseMessage response = await client.SendAsync(httpRequest);
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<DeckResponse>())!;
    }

    public async Task DeleteDeckAsync(int id)
    {
        HttpRequestMessage httpRequest = new(HttpMethod.Delete, $"/decks/{id}");
        userService.AddAuthorizationHeader(httpRequest);
        
        HttpResponseMessage response = await client.SendAsync(httpRequest);
        response.EnsureSuccessStatusCode();
    }
}