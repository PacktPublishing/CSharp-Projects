using CardTracker.Contracts.Requests;
using CardTracker.Contracts.Responses;

namespace CardTrackerClient.Services;

public interface ICardApiService
{
    Task<List<DeckResponse>> GetAllDecksAsync();
    Task<DeckResponse> GetDeckByIdAsync(int id);
    Task<DeckResponse> CreateDeckAsync(string name);
    Task<DeckResponse> UpdateDeckAsync(int id, EditDeckRequest request);
    Task DeleteDeckAsync(int id);
    Task<List<CardResponse>> GetAllCardsAsync();
}