namespace CardTrackerWebApi.Responses;

public class DeckResponse
{
    public int Id { get; init; }
    public required string Name { get; init; }
    
    public required int UserId { get; init; }
    
    public required List<CardDeck> CardDecks { get; init; }
}