namespace CardTrackerWebApi.Models;

public class CardDeck
{
    public required int CardId { get; set; }
    public required int DeckId { get; set; }
    public int Count { get; set; } = 1;
}