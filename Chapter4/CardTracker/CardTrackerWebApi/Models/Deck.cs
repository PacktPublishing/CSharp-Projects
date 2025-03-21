using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardTrackerWebApi.Models;

public class Deck
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required string Name { get; set; }
    
    public required int UserId { get; set; }
    
    public List<Card> Cards { get; set; } = new();
    public List<CardDeck> CardDecks { get; set; } = new();
}