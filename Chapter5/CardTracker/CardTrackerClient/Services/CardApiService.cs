using System.Net.Http.Json;
using CardTracker.Contracts.Requests;
using CardTracker.Contracts.Responses;

namespace CardTrackerClient.Services;

public interface ICardApiService
{
    Task<List<DeckResponse>> GetAllDecksAsync();
    Task<DeckResponse> GetDeckByIdAsync(int id);
    Task<DeckResponse> CreateDeckAsync(CreateDeckRequest request);
    Task<DeckResponse> UpdateDeckAsync(int id, EditDeckRequest request);
    Task DeleteDeckAsync(int id);
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
}

public class CardApiService(HttpClient client) : ICardApiService
{
    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("/login", loginRequest);

        response.EnsureSuccessStatusCode();

        LoginResponse loginResponse = (await response.Content.ReadFromJsonAsync<LoginResponse>())!;

        // Store the token into a token repository
        
        return loginResponse;
    }
    
    public async Task<List<DeckResponse>> GetAllDecksAsync()
    {
        HttpRequestMessage request = new(HttpMethod.Get, "/decks/");

        HttpResponseMessage response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<List<DeckResponse>>())!;
    }

    public async Task<DeckResponse> GetDeckByIdAsync(int id)
    {
        HttpRequestMessage request = new(HttpMethod.Get, $"/decks/{id}");

        HttpResponseMessage response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<DeckResponse>())!;
    }

    public async Task<DeckResponse> CreateDeckAsync(CreateDeckRequest request)
    {
        HttpRequestMessage httpRequest = new(HttpMethod.Post, "/decks")
        {
            Content = JsonContent.Create(request)
        };
        httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer");

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

        HttpResponseMessage response = await client.SendAsync(httpRequest);
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<DeckResponse>())!;
    }

    public async Task DeleteDeckAsync(int id)
    {
        HttpRequestMessage request = new(HttpMethod.Delete, $"/decks/{id}");

        HttpResponseMessage response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}