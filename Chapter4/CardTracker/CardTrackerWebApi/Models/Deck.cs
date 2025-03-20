using System.ComponentModel.DataAnnotations;

namespace CardTrackerWebApi.Models;

public record Deck
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
    
    public required User User { get; set; } = null!;
    
    public List<Card> Cards { get; set; } = new();
    public List<CardDeck> CardDecks { get; set; } = new();
}