using System.Net.Http.Json;
using CardTracker.Contracts.Requests;
using CardTracker.Contracts.Responses;

namespace CardTrackerClient.Services;

public class CardApiService(HttpClient client, IUserService userService) : ICardApiService
{

    public async Task<List<CardResponse>> GetAllCardsAsync()
    {
        HttpRequestMessage httpRequest = new(HttpMethod.Get, "/cards/");

        HttpResponseMessage response = await client.SendAsync(httpRequest);
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<List<CardResponse>>())!;
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

    public async Task<DeckResponse> CreateDeckAsync(string name)
    {
        HttpRequestMessage httpRequest = new(HttpMethod.Post, "/decks")
        {
            Content = JsonContent.Create(new CreateDeckRequest
            {
                Name = name
            })
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