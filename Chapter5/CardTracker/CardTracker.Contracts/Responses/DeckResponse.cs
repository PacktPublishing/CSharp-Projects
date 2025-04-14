namespace CardTracker.Contracts.Responses;

public class DeckResponse
{
    public int Id { get; init; }
    public required string Name { get; init; }
    
    public required int UserId { get; init; }
    
    public required List<CardDeck> CardDecks { get; init; }
}

public class CardDeck
{
    public required int CardId { get; init; }
    public required int DeckId { get; init; }
    public int Count { get; set; } = 1;
}